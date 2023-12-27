using BusinessLogic.Contracts;
using BusinessLogic.Models;
using Castle.Core.Resource;
using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Entity.Infrastructure;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Utilities;

[assembly: InternalsVisibleTo("UnitTests")]
namespace BusinessLogic.Services
{
    internal class SignInUserService : BaseService, ISignInUserService
    {
        private readonly IUserRepository? _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailValidationRepository _emailValidationRepository;
        private readonly ITFAService _tfaService;
        private readonly IJWTService _jwtService;
        private readonly ILogger<SignInUserService> _logger;
        private readonly ISmsService _smsService;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IDocumentsRepository _documentsRepository;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IParametersRepository _parametersRepository;

        public SignInUserService(IUserRepository userRepository, IConfiguration configuration, IEmailValidationRepository emailValidationRepository, ITFAService tfaService, IJWTService jwtService, ISmsService smsService, IUserInfoRepository userInfoRepository, IDocumentsRepository documentsRepository, IAzureBlobService azureBlobService, IParametersRepository parametersRepository, ILogger<SignInUserService> logger) : base(configuration)
        {
            this._userRepository = userRepository;
            this._configuration = configuration;
            this._emailValidationRepository = emailValidationRepository;
            this._tfaService = tfaService;
            this._jwtService = jwtService;
            this._smsService = smsService;
            this._userInfoRepository = userInfoRepository;
            this._documentsRepository = documentsRepository;
            this._azureBlobService = azureBlobService;
            this._parametersRepository = parametersRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Validate user credentials to do login
        /// </summary>
        /// <param name="credentials">Data dictionary with user info with keys [Email] and [Password] </param>
        /// <returns>A boolean that represents it was successfully or not</returns>
        public async Task<Response> SignIn(Dictionary<string, object> credentials)
        {
            try
            {
                var cellphone = credentials["Cellphone"].ToString();
                var encryptPassword = credentials["Password"].ToString();
                //var decryptPassword = this.DecodeString(encryptPassword, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
                var decryptPassword = this.DecodeString(encryptPassword!, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
                var userPassword = HashString.GenerateHashString(decryptPassword);

                var user = this._userRepository!.InstanceObject();
                user.CELLPHONE = cellphone;
                user.EMAIL = cellphone;
                //var userStored = this._userRepository.FindByCellphone(user);
                var userStored = await this._userRepository.FindByCellphoneOrEmail(user);

                int? intentos = userStored.ATTEMPTS == null ||  userStored.ATTEMPTS == 0  ? 1 + 1 : userStored.ATTEMPTS > 5 ? 5 : userStored.ATTEMPTS + 1;

                DateTime? DateLogger = userStored.UPDATED_AT;
                DateLogger = DateLogger?.AddMinutes(15);
                DateTime timeNow = DateTime.Now;

                if (userStored == null)
                {
                    return new Response(false, "Usuario/contraseña invalida");
                }

                if (userStored.ATTEMPTS >= 5 && DateLogger >= timeNow)
                {
                    return new Response(false, "Ha exedido el numero de intentos intente mas tarde");
                }
                else if(userStored.ATTEMPTS >= 5 && timeNow >= DateLogger)
                {
                    intentos = 1;
                }

                if (!((userStored.CELLPHONE!.Equals(cellphone) || userStored.EMAIL!.Equals(cellphone)) && (userStored.PASSWORD!.Equals(userPassword)))) 
                {
                    userStored.ATTEMPTS = intentos;
                    userStored.UPDATED_AT = timeNow;
                    this._userRepository.Update(userStored);
                    _userRepository.Save();
                    return new Response(false, "Usuario/contraseña invalida");
                }
                

                if (userStored.CELLPHONE_VALIDATED_AT == null)
                {
                    userStored.ATTEMPTS = intentos;
                    userStored.UPDATED_AT = timeNow;
                    this._userRepository.Update(userStored);
                    _userRepository.Save();
                    return new Response(false, "Este usuario no fue validado, por favor intenta recuperar tu contraseña");
                }
                user.CELLPHONE = userStored.CELLPHONE;
                userStored.LOGIN = 0;                
                var userinfo = _userRepository.UserInformation(user);
                var token = await this._jwtService.Authenticate(userinfo);
                userStored.TOKEN = (string)token["token"];
                userStored.ATTEMPTS = 1;
                userStored.UPDATED_AT = timeNow;
                _userRepository.Update(userStored);
                _userRepository.Save();

                return new Response(true, "Login exitoso", token);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        public async Task<Response> SignOut(Dictionary<string, object> credentials)
        {
            try
            {
                var userEmail = credentials["Cellphone"].ToString();

                var user = _userRepository.InstanceObject();
                user.CELLPHONE = userEmail;

                var userStored = this._userRepository.FindByCellphone(user);

                if (userStored != null && userStored.TOKEN != null)
                {
                    userStored.TOKEN = null;

                    _userRepository.Update(userStored);
                    var result = _userRepository.Save();
                    if (result.Success)
                    {
                        return new Response(true, "Logout exitoso");
                    }

                    return new Response(false, "Logout no exitoso");
                }

                return new Response(false, "No se encontro usuario con session activa");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        private string PrivateKeyFilePath(string nameFile)
        {
            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nameFile);

            return fullpath;
        }

        private string DecodeString(string encryptString, string privateKeyPath)
        {
            var rsaProvider = RSA.Create();
            var key = File.ReadAllText(privateKeyPath);
            rsaProvider.ImportFromPem(key);
            var bytes = rsaProvider.Decrypt(Convert.FromBase64String(encryptString), RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<Response> Profile(Dictionary<string, object> credentials)
        {
            try
            {
                var userIntance = this._userRepository!.InstanceObject();
                userIntance.CELLPHONE = (string)credentials["Cellphone"];
                var userinfo = await this._userRepository.FindByCellphoneOrEmail(userIntance);

                if (userinfo == null || userinfo.TUSERSINFO == null)
                {
                    return new Response(false, "No se encontro información del perfil");
                }                

                var result = new
                {
                    names = this.ToTitle(userinfo.TUSERSINFO.NAMES!),
                    lastname = this.ToTitle(userinfo.TUSERSINFO.LASTNAME!),
                    lastname2 = this.ToTitle(userinfo.TUSERSINFO.LASTNAME2!),
                    gender = userinfo.TUSERSINFO.IDGENDERNavigation == null ? "" : userinfo.TUSERSINFO.IDGENDERNavigation.DESCRIPTION,
                    email = userinfo.EMAIL == null ? "" : userinfo.EMAIL,
                    cellphone = userinfo.CELLPHONE!,//$"{userinfo.CELLPHONE![0..3]}-{userinfo.CELLPHONE[3..6]}-{userinfo.CELLPHONE[6..]}",
                    emailSecondary = userinfo.EMAILSECONDARY,
                    cellphoneSecondary = userinfo.CELLPHONESECONDARY
                };

                var response = new Response(true, result);

                return response;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Usuario no autorizado") throw new Exception(ex.Message);

                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        public async Task<Response> ChangePassword(Dictionary<string, object> credentials)
        {
            try
            {
                var userIntance = this._userRepository!.InstanceObject();
                userIntance.CELLPHONE = (string)credentials["Cellphone"];
                var code = (string)credentials["Code"];
                var userinfo = this._userRepository.FindByCellphone(userIntance);

                if (userinfo == null || userinfo.TUSERSINFO == null)
                {
                    return new Response(false, "No se encontro información del perfil");
                }

                var encryptOldPassword = credentials["OldPassword"].ToString();
                //var decryptOldPassword = this.DecodeString(encryptOldPassword, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
                var decryptOldPassword = this.DecodeString(encryptOldPassword!, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
                var userOldPassword = HashString.GenerateHashString(decryptOldPassword);

                if (userinfo.PASSWORD != userOldPassword)
                {
                    return new Response(false, "El password actual no coincide");
                }


                var resultService = await this._smsService.ValidateSmsOtp(userIntance.CELLPHONE, code);

                if (resultService.ContainsKey("status") && resultService["status"].ToString()!.ToLower() == "true")
                {
                    var encryptNewPassword = credentials["NewPassword"].ToString();
                    //var decryptNewPassword = this.DecodeString(encryptNewPassword, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
                    var decryptNewPassword = this.DecodeString(encryptNewPassword!, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
                    Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[.#?!@$%^&*-]).{8,12}$");

                    if (!validateGuidRegex.IsMatch(decryptNewPassword)) throw new Exception("Password no valido.");

                    var userNewHashPassword = HashString.GenerateHashString(decryptNewPassword);

                    userinfo.PASSWORD = userNewHashPassword;

                    this._userRepository.Update(userinfo);
                    var result = this._userRepository.Save();

                    if (result.Success)
                    {
                        return new Response(true, "Cambio de contraseña exitoso.");
                    }

                    return new Response(false, "No se logro realizar el cambio de contraseña, intentalo nuevamente.");
                }
                else
                {

                    return new Response(false, "La validación no ha sido exitosa");
                }


            }
            catch (Exception ex) {

                this._logger.LogError(ex.Message, ex);
                if (ex.Message == "Password no valido.")
                    return new Response(false, ex.Message);

                return new Response(false, "No se logro realizar el cambio de contraseña, intentalo nuevamente.");
            }           

        }

        public async Task<Response> ChangeEmail(Dictionary<string, object> credentials)
        {
            var userIntance = this._userRepository.InstanceObject();
            userIntance.EMAIL = (string)credentials["Email"];
            var userinfo =  this._userRepository.FindByEmail(userIntance);

            if (userinfo == null || userinfo.TUSERSINFO == null)
            {
                return new Response(false, "Informacion no valida");
            }

            var encryptPassword = (string)credentials["Password"];
            //var decryptPassword = this.DecodeString(encryptPassword, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
            var decryptPassword = this.DecodeString(encryptPassword, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
            var userPassword = HashString.GenerateHashString(decryptPassword);

            if (!userinfo.PASSWORD.Equals(userPassword))
            {
                return new Response(false, "Contraseña no es valida");
            }

            string newEmail = (string)credentials["NewEmail"];

            userinfo.EMAIL = newEmail;
            userinfo.EMAIL_VALIDATED_AT = null;
            this._userRepository.Update(userinfo);
            var saved = this._userRepository.Save();

            if (saved.Success)
            {
                var parameters = this._parametersRepository.GetParameters("EMAILCONFIRMATION");

                //var subject = _configuration.GetValue<string>("EmailConfirmation:Subject");
                var subject = parameters["SUBJECT"].ToString();
                //var fromemail = _configuration.GetValue<string>("EmailConfirmation:FromEmail");
                var fromemail = parameters["FROMEMAIL"].ToString();

                string hashcode = HashString.GenerateHashString(userinfo.EMAIL + Guid.NewGuid() + new Random(((int)DateTime.Now.Ticks)).Next(10000000));
                //var url = string.Format(_configuration.GetValue<string>("EmailConfirmation:BaseUrlProperty"), userinfo.EMAIL, hashcode);
                var url = string.Format(parameters["BASEURLPROPERTY"].ToString(), userinfo.EMAIL, hashcode);

                List<string> toemails = new List<string>();
                toemails.Add(userinfo.EMAIL);

                this.EmailConfirmation(toemails, new List<string>(), subject, fromemail, url, true, null);

                var emailcode = this._emailValidationRepository.InstanceObject();
                emailcode.IDUSER = userinfo.ID;
                emailcode.TOKEN = hashcode;
                emailcode.EMAIL = userinfo.EMAIL;
                this._emailValidationRepository.Insert(emailcode);
                var savedemail = this._emailValidationRepository.Save();

                return new Response(true, "EL cambio se ha realizado, valida tu correo nuevo con el email que se ha enviado.");
            }

            return new Response(false, "No se encontro información del perfil");
        }

        public async Task<Response> UpDoubleAuth(Dictionary<string, object> data)
        {
            string email = (string)data["Cellphone"];
            var parameters = this._parametersRepository.GetParameters("TWPFACTORAUTHENTICATION");
            //string secretcode = this._configuration.GetValue<string>("TwpFactorAuthentication:TwoFactorSecretCode");
            string secretcode = parameters["TWOFACTORSECRETCODE"].ToString();
            //string apptitle = this._configuration.GetValue<string>("TwpFactorAuthentication:AppTitle");
            string apptitle = parameters["APPTITLE"].ToString();
            var result = await this._tfaService.GenerateRegisterCode(email, secretcode, apptitle);

            return result;
        }


        public async Task<Response> UpDoubleAuthStatus(Dictionary<string, object> data)
        {
            string email = (string)data["Cellphone"];
            var userIntance = this._userRepository.InstanceObject();
            userIntance.CELLPHONE = email;
            string code = (string)data["Code"];

            var isValid = this.ValidateDoubleAuth(data);

            if (isValid.Result.Success == false)
            {
                return new Response(false, "Codigo no valido");
            }


            var userStored = this._userRepository.FindByCellphone(userIntance);

            if (userIntance != null)
            {
                userStored.REQUIREDCODE = true;

                this._userRepository.Update(userStored);

                var saved = this._userRepository.Save();


                if (saved.Success)
                {
                    return new Response(true, "Autenticacion de dos pasos activada.");
                }

                return new Response(false, "Autenticacion de dos pasos no se ha podido activar."); ;
            }

            return new Response(false, "Error inesperado."); ;
        }

        public async Task<Response> DownDoubleAuthStatus(Dictionary<string, object> data)
        {
            string email = (string)data["Cellphone"];
            var userIntance = this._userRepository.InstanceObject();
            userIntance.CELLPHONE = email;
            string code = (string)data["Code"];

            var isValid = await this.ValidateDoubleAuth(data);

            if (isValid.Success == false)
            {
                return new Response(false, "Codigo no valido");
            }

            var userStored = this._userRepository.FindByCellphone(userIntance);

            if (userIntance != null)
            {
                userStored.REQUIREDCODE = false;

                this._userRepository.Update(userStored);

                var saved = this._userRepository.Save();


                if (saved.Success)
                {
                    return new Response(true, "Autenticacion de dos pasos activada.");
                }

                return new Response(false, "Autenticacion de dos pasos no se ha podido activar."); ;
            }

            return new Response(false, "Error inesperado."); ;
        }

        public Task<Response> ValidateDoubleAuth(Dictionary<string, object> data)
        {
            string email = (string)data["Cellphone"];
            //string secretcode = this._configuration.GetValue<string>("TwpFactorAuthentication:TwoFactorSecretCode");
            string secretcode = this._parametersRepository.GetParameter<string>("TWPFACTORAUTHENTICATION", "TWOFACTORSECRETCODE");
            string appcode = (string)data["Code"];

            var result = this._tfaService.ValidateCode(email, secretcode, appcode);

            return result;
        }

        public async Task<Response> ChangeCellphone(Dictionary<string, object> data)
        {
            //string email = (string)data["Email"];
            string cellphone = (string)data["Cellphone"];
            string newCellphone = (string)data["NewCellphone"];

            var userIntance = this._userRepository.InstanceObject();
            userIntance.CELLPHONE = cellphone;

            var userStored = this._userRepository.FindByCellphone(userIntance);

            if (userStored == null)
            {
                this._logger.LogError("No se ha encontrado el email del usuario");
                return new Response(false, "Informacion no valida");
            }

            if (!userStored.CELLPHONE.Equals(cellphone))
            {
                this._logger.LogError($"Celular actual invalido al tratar de cambiar el telefono del usuario {cellphone}.");
                return new Response(false, "Informacion no valida");
            }

            userStored.CELLPHONE = newCellphone;
            userStored.CELLPHONE_VALIDATED_AT = DateTime.Now;

            this._userRepository.Update(userStored);
            this._userRepository.Save();

            this._logger.LogInformation($"El usuario {cellphone} ha cambiado su numero celular {cellphone} por {newCellphone}");
            return new Response(true, "Cambio de celular exitoso.");
        }

        public async Task<Response> ChangeCellphoneSmsCode(Dictionary<string, object> data)
        {
            //string email = (string)data["Email"];
            string cellphone = (string)data["Cellphone"];
            string newCellphone = (string)data["NewCellphone"];

            var userIntance = this._userRepository.InstanceObject();
            userIntance.CELLPHONE = cellphone;

            var userStored = this._userRepository.FindByCellphone(userIntance);

            if (userStored == null)
            {
                this._logger.LogError($"No se ha encontrado el usuario {cellphone}");
                return new Response(false, "Informacion no valida");
            }

            if (!userStored.CELLPHONE.Equals(cellphone))
            {
                this._logger.LogError($"Celular actual invalido al tratar de cambiar el telefono del usuario {cellphone}.");
                return new Response(false, "Informacion no valida");
            }


            this._logger.LogInformation($"El usuario {cellphone} ha cambiado su numero celular {cellphone} por {newCellphone}");
            return new Response(true, "Cambio de celular exitoso.");
        }

        public async Task<Response> SendChangeCellphoneCodeSms(Dictionary<string, object> user)
        {
            try
            {
                //string email = user["Email"].ToString();
                string cellphone = user["Cellphone"].ToString();
                string newcellphone = user["NewCellphone"].ToString();

                var userObject = _userRepository.InstanceObject();
                userObject.CELLPHONE = cellphone;

                var userStored = _userRepository.FindByCellphone(userObject);

                if (userStored == null)
                {
                    this._logger.LogError($"El correo no fue encontrado {userObject.CELLPHONE} en un intento de cambio de telefono");
                    return new Response(false, "Informacion no encontada");
                }

                if (!userStored.CELLPHONE.Equals(cellphone))
                {
                    this._logger.LogError($"El telfono anterior no es valido del usuario {userObject.CELLPHONE} en un intento de cambio de telefono");
                    return new Response(false, "Informacion no valida");
                }

                //var dic = new Dictionary<string, object>();
                //dic.Add("method", "post");
                //dic.Add("action", "otp_fina");

                //var body = UrlRequests.CreateSmsBodyRequestJson(newcellphone, this._configuration.GetValue<string>("OTPServices:Message"), 4);
                //var response = await UrlRequests.PostApiJsonRequest(this._configuration.GetValue<string>("OTPServices:MessengingService"), body, dic);

                var response = await this._smsService.SendSmsOtp(newcellphone);

                return new Response(true, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        public async Task<Response> ValidateChangeCellphoneSmsCode(Dictionary<string, object> user)
        {
            try
            {
                //string email = user["Email"].ToString();
                string cellphone = user["Cellphone"].ToString();
                string newcellphone = user["NewCellphone"].ToString();
                string code = (string)user["Code"];

                var userObject = _userRepository.InstanceObject();
                userObject.CELLPHONE = cellphone;
                var userStored = _userRepository.FindByCellphone(userObject);

                if (userStored == null)
                {
                    this._logger.LogError($"El correo no fue encontrado {cellphone} en un intento de validar el codigo de telefono");
                    return new Response(false, "Informacion no encontada");
                }

                if (!userStored.CELLPHONE.Equals(cellphone))
                {
                    this._logger.LogError($"El telefono anterior no es valido del usuario {userObject.EMAIL} en un intento de validacion de telefono");
                    return new Response(false, "Informacion no valida");
                }

                //var headers = new Dictionary<string, object>();
                //headers.Add("action", "check_fina_otp");
                //string url = string.Format(this._configuration.GetValue<string>("OTPServices:ValidationService"), newcellphone, code);
                //var response = await UrlRequests.MakeRequest<Dictionary<string, object>>(url, HttpMethod.Post, new Dictionary<string, object>(), headers);
                //var resultService = UrlRequests.CreateObjectFromJson<Dictionary<string, object>>(response);

                var resultService = await this._smsService.ValidateSmsOtp(newcellphone, code);

                if (resultService.ContainsKey("status") && resultService["status"].ToString().ToLower() == "true")
                {
                    return new Response(true, "Se ha valido correctamente el nuevo número telefónico");
                }

                return new Response(false, "La validación no ha sido exitosa");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }

        }

        public async Task<Response> ContractInfo(Dictionary<string, object> dictionary)
        {
            var cellphone = (string)dictionary["Cellphone"];
            var userIntance = this._userRepository.InstanceObject();
            userIntance.CELLPHONE = cellphone;

            var userStored = this._userRepository.FindByCellphone(userIntance);

            if (userStored == null)
            {
                return new Response(false, "Informacion invalida");
            }

            if (userStored.TUSERSINFO == null)
            {
                return new Response(false, "Error con el registro por favor contactar con soporte");
            }

            var email = (string)dictionary["Email"];//
            var occupation = (int)dictionary["Occupation"];//
            var address = (string)dictionary["Address"];//
            var rfc = (string)dictionary["Rfc"];//
            var maritalstatus = (int)dictionary["MaritalStatus"];//
            var rfcdoc = (string)dictionary["RfcDoc"];//
            var hometown = (string)dictionary["Hometown"];//
            var rfctype = (bool)dictionary["RfcType"];
            var typerfcfile = (string)dictionary["TypeFileDoc"];

            userStored.EMAIL = email;
            userStored.TUSERSINFO.RFCDOC = (rfcdoc != null ? true : false);
            userStored.TUSERSINFO.RFC = rfc;
            userStored.TUSERSINFO.IDOCCUPATION = occupation;
            userStored.TUSERSINFO.HOMETOWN = hometown;
            //userStored.TUSERSINFO.ADDRESS = address;
            userStored.TUSERSINFO.IDMARITALSTATUS = maritalstatus;
            userStored.TUSERSINFO.RFCTYPE = rfctype;

            if (!string.IsNullOrEmpty(rfcdoc))
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(rfcdoc)))
                {
                    stream.Position = 0;
                    string folder = HashString.GenerateHashString(cellphone);
                    string hashName = HashString.GenerateHashString(String.Format("CONSTANCIAFISCAL-{0}", new Random().Next(int.MinValue, int.MaxValue).ToString()));

                    string fullPath = Path.Combine(folder, hashName);
                    var CONTAINERNAMEDOCUMENTS = this._parametersRepository.GetParameter<string>("BLOBSTORAGE", "CONTAINERNAMEDOCUMENTS");
                    var blobresult = await this._azureBlobService.UploadFileStream(stream, fullPath, CONTAINERNAMEDOCUMENTS);


                    if (blobresult.Count > 0)
                    {
                        var newDocument = this._documentsRepository.InstanceObject();
                        newDocument.PATH = (string)blobresult["FullPath"];
                        newDocument.TITLE = "Contancia fiscal";
                        newDocument.URI = (string)blobresult["Url"];
                        newDocument.DESCRIPTION = "Constancia fiscal tomado del registro de confidencialidad";
                        newDocument.IDSTATUS = 1;
                        newDocument.IDUSER = userStored.ID;
                        newDocument.STATUSBLOB = 1; // 
                        newDocument.FILEEXTENCION = typerfcfile;
                        newDocument.IDDOCUMENTTYPE = 3; // id 3 ya que es documento tipo contancia fiscal
                        newDocument.CREATE_AT = DateTime.Now;

                        this._documentsRepository.Insert(newDocument);
                        var docsaveresult = this._documentsRepository.Save();

                        if (!docsaveresult.Success)
                        {
                            return new Response(false, "Ha ocurrido un error al guardar la contancia fiscal");
                        }
                    }
                }
            }

            this._userRepository.Update(userStored);
            var savedResult = this._userRepository.Save();

            if (savedResult.Success)
            {
                return new Response(true, "Guardado exitoso");
            }

            return new Response(false, "Ha ocurrido un error");
        }

        public async Task<Response> NDASignedStatus(Dictionary<string, object> dictionary)
        {
            var cellphone = dictionary["Cellphone"].ToString();

            var userinstance = this._userRepository.InstanceObject();
            userinstance.CELLPHONE = cellphone;
            var userstored = this._userRepository.FindByCellphone(userinstance);

            //if (userstored == null)
            //{
            //    return new Response(false, "No se encontro informacion valida");
            //}

            //if (userstored.WEBDOXREQUEST.Count > 0)
            //{
            //    var lastrequest = userstored.WEBDOXREQUEST.OrderByDescending(x => x.FORMWEBDOXID).OrderByDescending(x => x.ORDER).FirstOrDefault();
            //    if (lastrequest.CREATE_AT.GetValueOrDefault().Subtract(DateTime.Now).TotalDays < 7)
            //    {
            //        return new Response(false, "ya existe un proceso vigente");
            //    }
            //}

            if (userstored.PROCESSCONTRACT == null)
            {
                return new Response(true, "continua", true);
            }

            var limitdate = DateTime.Now - userstored.PROCESSCONTRACT;

            if (
                //limitdate.Value.TotalDays < this._configuration.GetValue<int>("WebdoxServices:ExpiredDays")
                limitdate.Value.TotalDays < this._parametersRepository.GetParameter<int>("WEBDOXSERVICES", "EXPIREDDAYS")
                )
            {
                return new Response(true, "en proceso", false);
            }

            else
            {
                if (userstored.CONTRACT_SIGN_AT != null)
                {
                    return new Response(true, "firmado", false);
                }

                return new Response(true, "el contrato ha caducado", true);

            }

        }

        public async Task<Response> UpdateDataUser(Dictionary<string, object> data)
        {
            try
            {
                var name = (string)data["Names"];
                var lastName = (string)data["Lastname"];
                var lastName2 = (string)data["Lastname2"];
                var email = (string)data["Email"];
                var cellphone = (string)data["Cellphone"];
                var emailSecondary = (string)data["EmailSecondary"];
                var cellphoneSecondary = (string)data["CellphoneSecondary"];
                var userIntance = this._userRepository!.InstanceObject();
                userIntance.EMAIL = (string)data["Email"];
                userIntance.CELLPHONE = (string)data["Cellphone"];
                var userinfo = await this._userRepository.FindByCellphoneOrEmail(userIntance);

                if (userinfo == null || userinfo.TUSERSINFO == null)
                {
                  return new Response(false, "Informacion no valida");
                }
                userinfo.TUSERSINFO.NAMES = string.IsNullOrEmpty(name) ? userinfo.TUSERSINFO.NAMES : name;
                userinfo.TUSERSINFO.LASTNAME = string.IsNullOrEmpty(lastName) ? userinfo.TUSERSINFO.LASTNAME : lastName;
                userinfo.TUSERSINFO.LASTNAME2 = string.IsNullOrEmpty(lastName2) ? userinfo.TUSERSINFO.LASTNAME2 : lastName2;
                userinfo.EMAIL = string.IsNullOrEmpty(email) ? userinfo.EMAIL : email;
                userinfo.CELLPHONE = string.IsNullOrEmpty(cellphone) ? userinfo.CELLPHONE : cellphone;
                userinfo.EMAILSECONDARY = string.IsNullOrEmpty(emailSecondary) ? userinfo.EMAILSECONDARY : emailSecondary;
                userinfo.CELLPHONESECONDARY = string.IsNullOrEmpty(cellphoneSecondary) ? userinfo.CELLPHONESECONDARY : cellphoneSecondary;
                this._userRepository.Update(userinfo);
                var result = this._userRepository.Save();
                if (result.Success)
                {
                   return new Response(true, "actualización datos exitoso.");
                }
                return new Response(false, "Ha ocurrido un error al actualizar datos.");
            }
            catch(Exception ex)
            {
                return new Response(false, "Ha ocurrido un error al actualizar datos." + ex);
            }
        }

        //public async Task<Response> GenerateContract(Dictionary<string, object> data)
        //{

        //    throw new NotImplementedException();

        //    string cellphone = (string)data["Cellphone"];
        //    var userIntance = this._userRepository.InstanceObject();
        //    userIntance.CELLPHONE = cellphone;

        //    var userStored = this._userRepository.FindByCellphone(userIntance);

        //    if (userStored == null)
        //    {
        //        return new Response(false, "Informacion no valida");
        //    }

        //    var ndapath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("NDASettings:FileName"));

        //    var ndacontent = File.ReadAllText(ndapath);

        //    string fullname = string.Format("{0} {1} {2}", userStored.TUSERSINFO.NAMES, userStored.TUSERSINFO.LASTNAME, userStored.TUSERSINFO.LASTNAME2);
        //    ndacontent = ndacontent.Replace("#NOMBRECOMPLETO", fullname.ToUpper());
        //    ndacontent = ndacontent.Replace("#OCUPACION", userStored.TUSERSINFO.IDOCCUPATIONNavigation.DESCRIPTION.ToUpper());
        //    ndacontent = ndacontent.Replace("#DOCUMENTO", "INE");
        //    ndacontent = ndacontent.Replace("#ENTIDADNACIMIENTO", userStored.TUSERSINFO.BIRTHPLACE.ToUpper());
        //    ndacontent = ndacontent.Replace("#FECHANACIMIENTO", userStored.TUSERSINFO.BIRTHDAY.GetValueOrDefault().ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-MX", false)).ToUpper());
        //    ndacontent = ndacontent.Replace("#DOCUMENTOEXPEDICION", "NO DEFINIDO");
        //    ndacontent = ndacontent.Replace("#FECHANACIMIENTO", userStored.TUSERSINFO.BIRTHDAY.GetValueOrDefault().ToLongDateString().ToUpper());
        //    ndacontent = ndacontent.Replace("#DOMICILIO", userStored.TUSERSINFO.ADDRESS.ToUpper());
        //    ndacontent = ndacontent.Replace("#RFC", userStored.TUSERSINFO.RFC.ToUpper());
        //    ndacontent = ndacontent.Replace("#FECHAACTUAL", DateTime.Now.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-MX", false)).ToUpper());
        //    ndacontent = ndacontent.Replace("#TELEFONO", userStored.CELLPHONE);
        //    ndacontent = ndacontent.Replace("#EMAIL", userStored.EMAIL);


        //    //var contract = ContractNDA.ContractNDA.CreateContent(ndacontent);

        //    //var contractresult = await this._azureBlobService.UploadFileStream(contract, "ContractV1");

        //    //if (contractresult.Count > 0)
        //    //{
        //    //    var newdocument = this._documentsRepository.InstanceObject();
        //    //    newdocument.TITLE = "Contrato v1";
        //    //    newdocument.DESCRIPTION = "Contrato en blanco sin firmas, con los datos del solicitante";
        //    //    newdocument.CREATE_AT = DateTime.Now;
        //    //    newdocument.URI = (string)contractresult["Url"];
        //    //    newdocument.PATH = (string)contractresult["FullPath"];
        //    //    newdocument.IDSTATUS = 1;
        //    //    newdocument.FILEEXTENCION = ".pdf";
        //    //    newdocument.STATUSBLOB = 1;
        //    //    newdocument.IDUSER = userStored.ID;
        //    //    newdocument.IDDOCUMENTTYPE = 4;



        //    //    this._documentsRepository.Insert(newdocument);
        //    //    var savedcontract = this._documentsRepository.Save();

        //    //    if (savedcontract.Success)
        //    //    {
        //    //        this._logger.LogInformation($"El contrato del usuario {userStored.ID} ha sido generado y guardado con exito");
        //    //    }

        //    //}

        //    //var contractBase64 = Convert.ToBase64String();


        //    return new Response(true, null);
        //}
    }
}
