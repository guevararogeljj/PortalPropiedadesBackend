using BusinessLogic.Contracts;
using BusinessLogic.Models;
using DataSource;
using DataSource.Contracts;
using LeadsInjection.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Utilities;
using WebDox;

namespace BusinessLogic.Services
{
    internal class SignUpUserService : BaseService, ISignUpUserService
    {
        #region Repositories and services
        private readonly IUserRepository _userRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IConfiguration _configuration;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IDocumentsRepository _documentsService;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IEmailValidationRepository _emailValidationRepository;
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly ISmsService _smsService;
        private readonly IWebdoxRequestService _webdoxRequestService;
        private readonly ILeadService _crmService;
        private readonly ILogger<SignUpUserService> _logger;
        #endregion

        #region Publics methods
        public SignUpUserService(IUserRepository userRepository, IUserInfoRepository userInfoRepository, IGenderRepository genderRepository, AppDbContext context, IConfiguration configuration, IAzureBlobService azureBlobService, IDocumentsRepository documentsService, IDocumentTypeRepository documentTypeRepository, IEmailValidationRepository emailValidationRepository, ILeadService crmService, IPasswordRecoveryRepository passwordRecoveryRepository, ISmsService smsService, IWebdoxRequestService webdoxRequestService, IParametersRepository parametersRepository, ILogger<SignUpUserService> logger) : base(configuration)
        {
            this._userRepository = userRepository;
            this._configuration = configuration;
            this._userInfoRepository = userInfoRepository;
            this._genderRepository = genderRepository;
            this._azureBlobService = azureBlobService;
            this._documentsService = documentsService;
            this._documentTypeRepository = documentTypeRepository;
            this._emailValidationRepository = emailValidationRepository;
            this._crmService = crmService;
            this._passwordRecoveryRepository = passwordRecoveryRepository;
            this._smsService = smsService;
            this._webdoxRequestService = webdoxRequestService;
            this._parametersRepository = parametersRepository;
            this._logger = logger;
        }

        public Response AddNewUser(Dictionary<string, object> data)
        {

            try
            {
                var names = (string)data["Name"];
                var lastname = (string)data["Lastname"];
                var lastname2 = (string)data["Lastname2"];
                var passwordEncrypt = (string)data["Password"];
                var cellphone = (string)data["Cellphone"];
                var email = (string)data["Email"];
                var userObject = _userRepository.InstanceObject();
                //var decryptpassword = this.DecodeString(passwordEncrypt, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
                var decryptpassword = this.DecodeString(passwordEncrypt, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
                Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[.#?!@$%^&*-]).{8,12}$");



                if (!validateGuidRegex.IsMatch(decryptpassword)) throw new Exception("El password no valido.");

                userObject.PASSWORD = HashString.GenerateHashString(decryptpassword);
                userObject.CELLPHONE = cellphone;
                userObject.CREATED_AT = DateTime.Now;
                userObject.EMAIL = email;
                var userinfo = this._userInfoRepository.InstanceObject();
                userinfo.NAMES = names;
                userinfo.LASTNAME = lastname;
                userinfo.LASTNAME2 = lastname2;

                var statusRegistration = _userRepository.GetStatusRegistration(userObject.ID, "SIGNUP");
                userObject.TRUSERSTATUSREGISTER.Add(statusRegistration);
                userObject.TUSERSINFO = userinfo;


                _userRepository.Insert(userObject);
                var result = _userRepository.Save();

                if (result.Success)
                {
                    string stringmessage = string.Format(StringResources.signinuserservice_addnewuser_success, cellphone);
                    this._logger.LogInformation(stringmessage);

                    var parameters = this._parametersRepository.GetParameters("CRMSETTINGS");

                    if (//this._configuration.GetValue<bool>("CrmSettings:Active")
                        Convert.ToBoolean(parameters["ACTIVE"]))
                    {
                        //string endpoint = this._configuration.GetValue<string>("CrmSettings:Url");
                        string endpoint = parameters["URL"].ToString();
                        //string formid = this._configuration.GetValue<string>("CrmSettings:Form_id");
                        string formid = parameters["FORM_ID"].ToString();
                        //string medio = this._configuration.GetValue<string>("CrmSettings:Medio");
                        string medio = parameters["MEDIO"].ToString();
                        //string apikey = this._configuration.GetValue<string>("CrmSettings:Omnicanalidad_key");
                        string apikey = parameters["OMNICANALIDAD_KEY"].ToString();
                        //string canal = this._configuration.GetValue<string>("CrmSettings:Canal");
                        string canal = parameters["CANAL"].ToString();
                        //string fuente = this._configuration.GetValue<string>("CrmSettings:Fuente");
                        string fuente = parameters["FUENTE"].ToString();

                        var infoclient = this._crmService.ClientInfoModel(string.Format("{0} {1} {2}", names, lastname, lastname), cellphone, string.Empty);

                        this._crmService.LeadInjection(endpoint, formid, medio, apikey, canal, fuente, infoclient);
                    }
                    return new Response(true, stringmessage);
                }

                this._logger.LogInformation(StringResources.signinuserservice_addnewuser_fail + cellphone);
                return new Response(false, StringResources.signinuserservice_addnewuser_fail);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);

                if(ex.Message == "El password no valido.")
                    return new Response(false, "El password no valido.");

                return new Response(false, StringResources.signinuserservice_addnewuser_exception);
            }

        }

        public async Task<Response> SendCodeEmail(Dictionary<string, object> data)
        {
            string cellphone = (string)data["Cellphone"];

            var userObject = this._userRepository.InstanceObject();
            userObject.CELLPHONE = cellphone;

            var userStored = this._userRepository.FindByCellphone(userObject);

            if (userStored == null)
            {
                this._logger.LogInformation($"No se ha encontrado el usuario: {userStored.CELLPHONE}, en el intento de envio de correo de verificacion.");
                return new Response(false, "No se ha encontrado la informacion solicitada.");
            }

            try
            {
                var parameters = this._parametersRepository.GetParameters("EMAILCONFIRMATION");

                //var subject = _configuration.GetValue<string>("EmailConfirmation:Subject");
                var subject = parameters["SUBJECT"].ToString();
                //var fromemail = _configuration.GetValue<string>("EmailConfirmation:FromEmail");
                var fromemail = parameters["FROMEMAIL"].ToString();

                string hashcode = HashString.GenerateHashString(userStored.EMAIL + Guid.NewGuid() + new Random(((int)DateTime.Now.Ticks)).Next(10000000));
                //var url = string.Format(_configuration.GetValue<string>("EmailConfirmation:BaseUrlProperty"), userStored.EMAIL, hashcode);
                var url = string.Format(parameters["BASEURLPROPERTY"].ToString(), userStored.EMAIL, hashcode);

                List<string> toemails = new List<string>();
                toemails.Add(userStored.EMAIL);
                this.EmailConfirmation(toemails, new List<string>(), subject, fromemail, url, true, data);

                var emailcode = this._emailValidationRepository.InstanceObject();
                emailcode.IDUSER = userStored.ID;
                emailcode.TOKEN = hashcode;
                emailcode.EMAIL = userStored.EMAIL;
                this._emailValidationRepository.Insert(emailcode);
                var savedemail = this._emailValidationRepository.Save();

                this._logger.LogInformation($"Se ha enviado correo de verificacion al email: {userStored.EMAIL} del usuario: {userStored.CELLPHONE}");
                return new Response(true, "Se ha enviado el correo de verificacion");
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Se ha generado un error al intentar de enviar el correo de validacion de correo al usuario: {userStored.CELLPHONE}", ex);
                this._logger.LogError(ex.Message);
                this._logger.LogError(ex.StackTrace);
                return new Response(false, "Ha ocurrido un error al tratar de enviar el correo de verificacion de email.");
            }
        }

        #region Sms validation
        public async Task<Object> SendCodeSms(Dictionary<string, object> user)
        {
            try
            {
                var userObject = _userRepository.InstanceObject();
                userObject.CELLPHONE = user["Cellphone"].ToString();

                var userStored = _userRepository.FindByCellphone(userObject);


                //ratelimit best practice add package
                int? intentos = userStored.ATTEMPTSCODE == null || userStored.ATTEMPTSCODE == 0 ? 1 + 1 : userStored.ATTEMPTSCODE > 5 ? 5 : userStored.ATTEMPTSCODE + 1;

                DateTime? DateLogger = userStored.UPDATED_AT;
                DateLogger = DateLogger?.AddMinutes(5);
                DateTime timeNow = DateTime.Now;

    
                if (userStored.ATTEMPTSCODE >= 5 && DateLogger >= timeNow)
                {
                    return new Response(false, "Ha exedido el numero de intentos intente mas tarde");
                }
                else if (userStored.ATTEMPTSCODE >= 5 && timeNow >= DateLogger)
                {
                    intentos = 1;
                }


                if (userStored == null)
                {
                    this._logger.LogError(StringResources.signinuserservice_sendcodesms_invalid_log, userObject.CELLPHONE);
                    return new Response(false, StringResources.signinuserservice_sendcodesms_invalid);
                }

                var cellphone = userStored.CELLPHONE;

                //string response = await SendCodeCellphone(cellphone);
                var response = await this._smsService.SendSmsOtp(cellphone!);
                userStored.ATTEMPTSCODE = intentos;
                userStored.UPDATED_AT = timeNow;
                this._userRepository.Update(userStored);
                this._userRepository.Save();
                return new Response(true, StringResources.signinuserservice_sendcodesms_success);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, StringResources.signinuserservice_sendcodesms_exception);
            }
        }

        //private async Task<string> SendCodeCellphone(string? cellphone)
        //{
        //    var dic = new Dictionary<string, object>();
        //    dic.Add("method", "post");
        //    dic.Add("action", "otp_fina");

        //    var body = UrlRequests.CreateSmsBodyRequestJson(cellphone, this._configuration.GetValue<string>("OTPServices:Message"), this._configuration.GetValue<int>("OTPServices:DigitsNumber"));
        //    var response = await UrlRequests.PostApiJsonRequest(this._configuration.GetValue<string>("OTPServices:MessengingService"), body, dic);
        //    return response;
        //}

        public async Task<Object> ValidateSmsCode(Dictionary<string, object> user)
        {
            try
            {
                string code = (string)user["Code"];
                var userObject = _userRepository.InstanceObject();
                userObject.CELLPHONE = (string)user["Cellphone"];
                userObject.EMAIL = (string)user["Cellphone"];
                var userStored = await _userRepository.FindByCellphoneOrEmail(userObject);

                if (userStored == null)
                {
                    this._logger.LogError(String.Format(StringResources.signinuserservice_validatesmscode_invalid_log, userObject.CELLPHONE));
                    return new Response(false, StringResources.signinuserservice_validatesmscode_invalid);
                }
                var cellphone = userStored.CELLPHONE;

                //var headers = new Dictionary<string, object>();
                //headers.Add("action", "check_fina_otp");

                //string url = string.Format(this._configuration.GetValue<string>("OTPServices:ValidationService"), cellphone, code);

                //var response = await UrlRequests.MakeRequest<Dictionary<string, object>>(url, HttpMethod.Post, new Dictionary<string, object>(), headers);

                //var resultService = UrlRequests.CreateObjectFromJson<Dictionary<string, object>>(response);
                //Dictionary<string, object> resultService = await SendValidateCodeCellphone(code, cellphone);

                var resultService = await this._smsService.ValidateSmsOtp(userStored.CELLPHONE!, code);

                if (resultService.ContainsKey("status") && resultService["status"].ToString().ToLower() == "true")
                {
                    userStored.CELLPHONE_VALIDATED_AT = DateTime.Now;
                    var statusRegistration = _userRepository.GetStatusRegistration(userStored.ID, "VALIDATEDCODE");
                    userStored.TRUSERSTATUSREGISTER.Add(statusRegistration);
                    _userRepository.Update(userStored);
                    var saved = _userRepository.Save();

                    return new Response(true, StringResources.signinuserservice_validatesmscode_success);
                }

                return new Response(false, StringResources.signinuserservice_validatesmscode_fail);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }

        }
        #endregion

        public object StatusRegisterUser(Dictionary<string, object> user)
        {
            try
            {
                var instanceUser = _userRepository.InstanceObject();
                instanceUser.EMAIL = user["Cellphone"].ToString();

                var storedUser = _userRepository.FindByEmail(instanceUser);

                var registerStatusUser = storedUser.TRUSERSTATUSREGISTER
                    .Select(x => x.IDREGISTERSTATUSNavigation)
                    .Max(x => new { id = x.ID, description = x.DESCRIPTION });

                return registerStatusUser;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        public async Task<object> ValidateIdentification(IEnumerable<Dictionary<string, object>> files)
        {
            try
            {
                var userEmail = files.FirstOrDefault();
                if (!userEmail.ContainsKey("Cellphone") || userEmail["Cellphone"] == null)
                {
                    return new Response(false, "La informacion no esta completa");
                }

                Dictionary<string, object> apiHeader = GetApiHeader();

                var instanceUser = _userRepository.InstanceObject();
                instanceUser.CELLPHONE = userEmail["Cellphone"].ToString();
                var userStored = _userRepository.FindByCellphone(instanceUser);

                if (userStored == null)
                {
                    return new Response(false, "Error en la consulta de usuario.");
                }

                if (userStored.TRUSERSTATUSREGISTER.Where(x => x.IDREGISTERSTATUSNavigation.DESCRIPTION.Equals("VALIDATEDID")).Any())
                {
                    return new Response(true, "El usuario ya tiene registrada una identificacion");
                }

                var parameters = this._parametersRepository.GetParameters("NUFISERVICES");

                foreach (var item in files)
                {
                    if (item.ContainsKey("FileType") && (string)item["FileType"] == "Front")
                    {
                        var body = new Dictionary<string, object> { { "base64_credencial_frente", item["FileBase64"] } };
                        //Dictionary<string, object> responseResult = await ApiRequest(apiHeader, body, this._configuration.GetValue<string>("NufiServices:IFEFront")); // call nufi ocr to get curp from ID
                        Dictionary<string, object> responseResult = await ApiRequest(apiHeader, body, parameters["IFEFRONT"].ToString()); // call nufi ocr to get curp from ID

                        if (responseResult.ContainsKey("status") && responseResult["status"].ToString() == "success")
                        {
                            var newUserInfo = _userInfoRepository.InstanceObject();
                            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(responseResult["data"].ToString()); // respuesta del ocr a la peticion de la ife
                            var userinfo = JsonSerializer.Deserialize<Dictionary<string, object>>(data["ocr"].ToString()); //seccion de ife de informacion de ife 
                            //var ifevigente = bool.Parse(data["vigente"].ToString()); // validacion de vigencia de IFE
                            var documentid = userinfo["clave"].ToString();
                            var documentplace = string.Format("{0},{1}", userinfo["municipio"].ToString(), userinfo["estado"].ToString());
                            var address = string.Format("{0}, {1}, {2} {3}, {4}", userinfo["calle_numero"], userinfo["colonia"], userinfo["codigo_postal"], userinfo["municipio"], userinfo["estado"]);

                            //if (!ifevigente)
                            //{
                            //    return new Response(false, "Informacion invalida o no vigente.");
                            //}

                            var dataCurp = await ValidateUserCurp(userinfo["curp"].ToString()); // call to api renapo to get info user from government site
                            var gender = _genderRepository.FindByDescription(dataCurp["sexo"].ToString()); //get type gender from catalog
                            //userinfo["estado"]  userinfo["municipio"]
                            if (userStored.TUSERSINFO != null)
                            {
                                userStored.TUSERSINFO.LASTNAME = dataCurp["primerApellido"].ToString();
                                userStored.TUSERSINFO.LASTNAME2 = dataCurp["segundoApellido"].ToString();
                                userStored.TUSERSINFO.NAMES = dataCurp["nombres"].ToString();
                                userStored.TUSERSINFO.BIRTHDAY = DateTime.Parse(dataCurp["fechaNacimiento"].ToString());
                                userStored.TUSERSINFO.IDGENDER = gender.ID;
                                userStored.TUSERSINFO.CURP = userinfo["curp"].ToString();
                                userStored.TUSERSINFO.BIRTHPLACE = dataCurp["entidad"].ToString();
                                userStored.TUSERSINFO.CREATED_AT = DateTime.Now;
                                userStored.TUSERSINFO.NUMBERDOCUMENT = documentid;
                                userStored.TUSERSINFO.PLACEDOCUMENT = documentplace;
                                userStored.TUSERSINFO.ADDRESS = address;
                            }
                            else
                            {
                                newUserInfo.LASTNAME = dataCurp["primerApellido"].ToString();
                                newUserInfo.LASTNAME2 = dataCurp["segundoApellido"].ToString();
                                newUserInfo.NAMES = dataCurp["nombres"].ToString();
                                newUserInfo.BIRTHDAY = DateTime.Parse(dataCurp["fechaNacimiento"].ToString());
                                newUserInfo.IDGENDER = gender.ID;
                                newUserInfo.CURP = userinfo["curp"].ToString();
                                newUserInfo.BIRTHPLACE = dataCurp["entidad"].ToString();
                                newUserInfo.CREATED_AT = DateTime.Now;
                                newUserInfo.NUMBERDOCUMENT = documentid;
                                newUserInfo.PLACEDOCUMENT = documentplace;
                                newUserInfo.ADDRESS = address;
                                userStored.TUSERSINFO = newUserInfo;
                            }


                            var statusRegistration = _userRepository.GetStatusRegistration(userStored.ID, "VALIDATEDID");
                            userStored.TRUSERSTATUSREGISTER.Add(statusRegistration);

                            var file = await SaveFileToStorage(userStored.CELLPHONE, item["FileBase64"].ToString(), item["FileName"].ToString()); //save to blobstorage ID image

                            if (file.Count > 0) // if blobstorage wass sucessfully save record to database
                            {
                                var docType = _documentTypeRepository.FindByDescription("IFE-FRONTAL");
                                var newFile = _documentsService.InstanceObject();
                                newFile.TITLE = item["FileName"].ToString();
                                newFile.URI = file["Url"].ToString();
                                newFile.PATH = file["FullPath"].ToString();
                                newFile.IDSTATUS = 1; // 1 up, 2 disable
                                newFile.IDDOCUMENTTYPE = docType.ID;
                                newFile.CREATE_AT = DateTime.Now;
                                newFile.FILEEXTENCION = item["FileName"].ToString().Split(".")[1];
                                userStored.TDOCUMENTS.Add(newFile);

                            }
                            else
                            {
                                return new Response(false, "No se pudo guardar el frente de la indentificación");
                            }
                        }

                        if (responseResult.ContainsKey("status") && responseResult["status"].ToString() == "error")
                        {
                            //todo: make error behaviour 
                            return new Response(false, "No se pudo leer los datos de la indentificación");
                        }

                    }


                    if (item.ContainsKey("FileType") && (string)item["FileType"] == "Back")
                    {
                        //var body = new Dictionary<string, object> { { "base64_credencial_reverso", item["FileBase64"] } };
                        //Dictionary<string, object> responseResult = await ApiRequest(apiHeader, body, this._configuration.GetValue<string>("NufiServices:IFEBack"));

                        //if (responseResult.ContainsKey("status") && responseResult["status"].ToString() == "success")
                        //{

                        var body = new Dictionary<string, object> { { "base64_credencial_reverso", item["FileBase64"] } };
                        //Dictionary<string, object> responseResult = await ApiRequest(apiHeader, body, this._configuration.GetValue<string>("NufiServices:IFEBack")); // call nufi ocr to get curp from ID
                        Dictionary<string, object> responseResult = await ApiRequest(apiHeader, body, parameters["IFEBACK"].ToString()); // call nufi ocr to get curp from ID

                        var file = await SaveFileToStorage(userStored.CELLPHONE, item["FileBase64"].ToString(), item["FileName"].ToString());

                        if (file.Count > 0)
                        {
                            var docType = _documentTypeRepository.FindByDescription("IFE-REVERSO");
                            var newFile = _documentsService.InstanceObject();
                            newFile.TITLE = item["FileName"].ToString();
                            newFile.URI = file["Url"].ToString();
                            newFile.PATH = file["FullPath"].ToString();
                            newFile.IDSTATUS = 1; // 1 up, 2 disable
                            newFile.IDDOCUMENTTYPE = docType.ID;
                            newFile.CREATE_AT = DateTime.Now;
                            newFile.FILEEXTENCION = item["FileName"].ToString().Split(".")[1];
                            userStored.TDOCUMENTS.Add(newFile);
                        }
                        else /*(responseResult.ContainsKey("status") && responseResult["status"].ToString() == "error")*/
                        {
                            //todo: make error behaviour 
                            return new Response(false, "No se pudo guardar el reverso de la indentificación ");
                        }
                    }
                }
                userStored.IDENTITY_VALIDATED_AT = DateTime.Now;
                _userRepository.Update(userStored);
                var userSaved = _userRepository.Save();

                if (userSaved.Success)
                {
                    //Dictionary<string, object> result = new Dictionary<string, object>();
                    //result.Add("Email", userStored.EMAIL);
                    //result.Add("Phone", userStored.CELLPHONE);
                    //result.Add("Fullname", $"{userStored.TUSERSINFO.NAMES} {userStored.TUSERSINFO.LASTNAME} {userStored.TUSERSINFO.LASTNAME2}");
                    //var crmresult = await this._crmService.ClientToCrm(result);

                    //this._logger.LogInformation($"Crm client send it {userStored.EMAIL}", crmresult.Result);

                    return new Response(true, "El proceso ha sido exitoso.");
                }

                return new Response(false, "No se logro capturar los datos, intentalo nuevamente.");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        //return new Response(false, "No se logro capturar los datos, intentalo nuevamente.");


        public async Task<Object> UserInformation(Dictionary<string, object> data)
        {

            var instanceUser = _userRepository.InstanceObject();
            instanceUser.CELLPHONE = data["Cellphone"].ToString();
            try
            {
                var storedUser = _userRepository.FindByCellphone(instanceUser);

                if (storedUser == null)
                {
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_userinformation_invalid_log, instanceUser.CELLPHONE));
                    return new Response(false, StringResources.signinuserservice_userinformation_invalid);
                }

                var storedUserInfo = storedUser.TUSERSINFO;

                if (storedUser == null)
                {
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_userinformation_fail_log, instanceUser.CELLPHONE));
                    return new Response(false, StringResources.signinuserservice_userinformation_fail);
                }

                var infoUser = new
                {
                    names = storedUserInfo.NAMES,
                    lastname = storedUserInfo.LASTNAME,
                    lastname2 = storedUserInfo.LASTNAME2,
                    birthday = storedUserInfo.BIRTHDAY.Value.ToShortDateString(),
                    birthplace = storedUserInfo.BIRTHPLACE,
                    gender = storedUserInfo.IDGENDERNavigation.DESCRIPTION
                };

                return new Response(true, StringResources.signinuserservice_userinformation_success, infoUser);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_userinformation_exception_log, instanceUser.CELLPHONE), ex.StackTrace);
                this._logger.LogError(ex.Message);
                this._logger.LogError(ex.StackTrace);
                return new Response(false, StringResources.signinuserservice_userinformation_exception);
            }
        }

        public async Task<Object> UpdateUserInformation(Dictionary<string, object> data)
        {
            try
            {
                var instanceUser = _userRepository.InstanceObject();
                instanceUser.CELLPHONE = data["Cellphone"].ToString();
                var storedUser = _userRepository.FindByCellphone(instanceUser);
                storedUser.TUSERSINFO.TERMS = (bool)data["Terms"];

                var saved = _userInfoRepository.Save();

                if (saved.Success)
                {
                    return new Response(true, "Actualizacion exitosa");
                }

                return new Response(false, "Ha fallado la actualizacion de datos");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return new Response(false, "Ha ocurrido un error");
            }
        }

        #endregion

        #region Private methods

        private async Task<Dictionary<string, object>> SaveFileToStorage(string pEmail, string pFile, string pFileName)
        {

            using (var stream = new MemoryStream(Convert.FromBase64String(pFile.Split(",")[1])))
            {
                stream.Position = 0;
                string folder = HashString.GenerateHashString(pEmail);
                string hashName = HashString.GenerateHashString(pFileName);

                string fullPath = Path.Combine(folder, hashName);
                var CONTAINERNAMEDOCUMENTS = this._parametersRepository.GetParameter<string>("BLOBSTORAGE", "CONTAINERNAMEDOCUMENTS");
                var file = await _azureBlobService.UploadFileStream(stream, fullPath, CONTAINERNAMEDOCUMENTS);

                return file;

            }
        }

        private async Task<Dictionary<string, object>> ValidateUserCurp(string pCurp)
        {
            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add("tipo_busqueda", "curp");
            body.Add("curp", pCurp);

            //Dictionary<string, object> responseResult = await ApiRequest(GetApiHeader(), body, this._configuration.GetValue<string>("NufiServices:CurpValidation"));
            Dictionary<string, object> responseResult = await ApiRequest(GetApiHeader(), body, this._parametersRepository.GetParameter<string>("NUFISERVICES", "CURPVALIDATION"));

            if (responseResult.Keys.Contains("status") && responseResult["status"].ToString() == "success")
            {
                var databody = JsonSerializer.Deserialize<Dictionary<string, object>>(responseResult["data"].ToString());
                var data = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(databody["curpdata"].ToString()).FirstOrDefault();

                return data;
            }

            return new Dictionary<string, object>();
        }

        private async Task<Dictionary<string, object>> ApiRequest(Dictionary<string, object> apiHeader, Dictionary<string, object> body, string url)
        {
            var response = await UrlRequests.MakeRequest<Dictionary<string, object>>(url, HttpMethod.Post, body, apiHeader);
            var responseResult = UrlRequests.CreateObjectFromJson<Dictionary<string, object>>(response);
            return responseResult;
        }

        private Dictionary<string, object> GetApiHeader()
        {
            var dic = new Dictionary<string, object>();
            var parameters = this._parametersRepository.GetParameters("NUFISERVICES");
            //dic.Add(this._configuration.GetValue<string>("NufiServices:ApiKeyName"), this._configuration.GetValue<string>("NufiServices:ApiKey"));
            dic.Add(parameters["APIKEYNAME"].ToString(), parameters["APIKEY"].ToString());
            return dic;
        }
        #endregion

        //public async Task<Response> EmailValidate(Dictionary<string, object> data)
        public async Task<Response> ValidateCodeEmail(Dictionary<string, object> data)
        {
            string emailParam = (string)data["Email"];
            string codeParam = (string)data["Code"];

            var userParam = this._userRepository.InstanceObject();
            userParam.EMAIL = emailParam;

            var userIntance = this._userRepository.FindByEmail(userParam);

            if (userIntance == null)
            {
                return new Response(false, "Link invalido.");
            }

            var emailtoken = userIntance.TEMAILVALIDATION.Where(x => x.TOKEN.Equals(codeParam)).FirstOrDefault();

            if (emailtoken == null || emailtoken.USED_AT.HasValue || emailtoken.IDUSERNavigation.EMAIL_VALIDATED_AT.HasValue)
            {
                return new Response(false, "Este link ha cadudado.");
            }

            var dateval = DateTime.Now;
            emailtoken.USED_AT = dateval;
            emailtoken.IDUSERNavigation.EMAIL_VALIDATED_AT = dateval;


            var user = this._userRepository.InstanceObject();
            user.EMAIL = emailtoken.EMAIL;
            var statusRegistration = _userRepository.GetStatusRegistration(userIntance.ID, "VALIDATEDEMAIL");
            var status = userIntance.TRUSERSTATUSREGISTER.Where(x => x.IDUSER == statusRegistration.IDUSER && x.IDREGISTERSTATUS == statusRegistration.IDREGISTERSTATUS).FirstOrDefault();
            if (status != null)
            {
                status.UPDATED_AT = DateTime.Now;
            }
            else
            {
                userIntance.TRUSERSTATUSREGISTER.Add(statusRegistration);
            }
            //this._emailValidationRepository.Update(emailtoken);
            this._userRepository.Update(userIntance);
            //var saved = this._emailValidationRepository.Save();
            var saved = this._userRepository.Save();

            if (saved.Success)
            {
                await this.StartWedoxFlow(userIntance.ID);

                return new Response(true, "Tu cuenta de correo ha sido validada.");
            }

            return new Response(false, "No se ha podido validar su cuenta.");
        }

        public async Task<Response> PasswordRecovery(Dictionary<string, object> data)
        {
            var cellphone = (string)data["Cellphone"];
            var userInstance = this._userRepository.InstanceObject();
            userInstance.CELLPHONE = cellphone;
            userInstance.EMAIL = cellphone;
            var userStored = await this._userRepository.FindByCellphoneOrEmail(userInstance);

            if (userStored == null)
            {
                return new Response(false, "Usuario/contraseña invalida.");
            }


            //var subject = _configuration.GetValue<string>("PasswordRecovery:Subject");
            //var fromemail = _configuration.GetValue<string>("PasswordRecovery:FromEmail");

            string hashcode = HashString.GenerateHashString(userStored.CELLPHONE + Guid.NewGuid() + new Random(((int)DateTime.Now.Ticks)).Next(10000000));
            //var url = string.Format(_configuration.GetValue<string>("PasswordRecovery:BaseUrlProperty"), userStored.EMAIL, hashcode);

            try
            {
                //List<string> toemails = new List<string>();
                //toemails.Add(userStored.EMAIL);
                //this.EmailConfirmation(toemails, new List<string>(), subject, fromemail, url, true, data);

                var passwordchange = this._passwordRecoveryRepository.InstanceObject();
                passwordchange.IDUSER = userStored.ID;
                passwordchange.TOKEN = hashcode;
                passwordchange.EMAIL = userStored.CELLPHONE!;
                this._passwordRecoveryRepository.Insert(passwordchange);

                var savepasswordrecovery = this._passwordRecoveryRepository.Save();
                data["Cellphone"] = userStored.CELLPHONE!;
                if (savepasswordrecovery.Success)
                {
                    var smsresult = this.SendCodeSms(data);
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_passwordrecovery_success_log, userStored.CELLPHONE!));
                    return new Response(true, StringResources.signinuserservice_passwordrecovery_success);
                }

                this._logger.LogInformation(string.Format(StringResources.signinuserservice_passwordrecovery_fail_log, cellphone), savepasswordrecovery.Message);
                return new Response(false, StringResources.signinuserservice_passwordrecovery_fail);

            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_passwordrecovery_exception_log, cellphone), ex);
                return new Response(false, StringResources.signinuserservice_passwordrecovery_exception);
            }
        }

        public async Task<Response> PasswordRecoveryValidate(Dictionary<string, object> data)
        {
            var cellphone = (string)data["Cellphone"];
            var code = (string)data["Code"];
            var password = (string)data["NewPassword"];

            var userInstance = this._userRepository.InstanceObject();
            userInstance.CELLPHONE = cellphone;
            userInstance.EMAIL = cellphone;
            var userStored = await this._userRepository.FindByCellphoneOrEmail(userInstance);

            if (userStored == null)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_invalid_log, cellphone));
                return new Response(false, StringResources.signinuserservice_passwordrecoveryvalidate_invalid);
            }


            var passwordRecovery = userStored.TPASSWORDRECOVERY
                .Where(x => x.EMAIL == userStored.CELLPHONE! && x.USED_AT == null && DateTime.Now < x.EXPIRATION_AT).FirstOrDefault();


            if (passwordRecovery == null)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_invalid2_log, cellphone));
                return new Response(false, StringResources.signinuserservice_passwordrecoveryvalidate_invalid2);
            }
            data["Cellphone"] = userStored.CELLPHONE!;
            cellphone = userStored.CELLPHONE!;
            var validatecode = await this.ValidateSmsCodePasswordRecovery(data);

            if (validatecode.Success == false)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_invalid3_log, cellphone));
                return new Response(false, StringResources.signinuserservice_passwordrecoveryvalidate_invalid3);
            }

            try
            {
                var encryptPassword = password;
                //var decryptPassword = this.DecodeString(encryptPassword, PrivateKeyFilePath(_configuration.GetValue<string>("Encryption:PrivateKeyFileName")));
                var decryptPassword = this.DecodeString(encryptPassword, PrivateKeyFilePath(this._parametersRepository.GetParameter<string>("ENCRYPTION", "PRIVATEKEYFILENAME")));
                var userNewPassword = HashString.GenerateHashString(decryptPassword);

                userStored.PASSWORD = userNewPassword;
                userStored.CELLPHONE_VALIDATED_AT = DateTime.Now;
                passwordRecovery.USED_AT = DateTime.Now;
                this._userRepository.Update(userStored);
                var saved = this._userRepository.Save();

                if (saved.Success)
                {
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_success_log, cellphone));
                    return new Response(true, StringResources.signinuserservice_passwordrecoveryvalidate_success);
                }

                this._logger.LogInformation(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_fail_log, cellphone), saved.Message);
                return new Response(false, StringResources.signinuserservice_passwordrecoveryvalidate_fail);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_passwordrecoveryvalidate_exception_log, cellphone), ex);
                return new Response(false, StringResources.signinuserservice_passwordrecoveryvalidate_exception);
            }
        }

        private async Task<Response> ValidateSmsCodePasswordRecovery(Dictionary<string, object> user)
        {
            var userObject = _userRepository.InstanceObject();
            userObject.CELLPHONE = user["Cellphone"].ToString();
            string code = (string)user["Code"];
            try
            {
                var userStored = _userRepository.FindByCellphone(userObject);

                if (userStored == null)
                {
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_validatesmscodepasswordrecovery_invalid_log, userObject.CELLPHONE));
                    return new Response(false, StringResources.signinuserservice_validatesmscodepasswordrecovery_invalid);
                }
                var cellphone = userStored.CELLPHONE;

                //Dictionary<string, object> resultService = await SendValidateCodeCellphone(code, cellphone);
                var resultService = await this._smsService.ValidateSmsOtp(cellphone, code);

                if (resultService.ContainsKey("status") && resultService["status"].ToString().ToLower() == "true")
                {
                    this._logger.LogInformation(string.Format(StringResources.signinuserservice_validatesmscodepasswordrecovery_success_log, cellphone));
                    return new Response(true, StringResources.signinuserservice_validatesmscodepasswordrecovery_success);
                }

                this._logger.LogError(string.Format(StringResources.signinuserservice_validatesmscodepasswordrecovery_fail_log, cellphone));
                return new Response(false, StringResources.signinuserservice_validatesmscodepasswordrecovery_fail);
            }
            catch (Exception ex)
            {
                this._logger.LogError(string.Format(StringResources.signinuserservice_validatesmscodepasswordrecovery_exception_log, userObject.CELLPHONE), ex);
                return new Response(false, StringResources.signinuserservice_validatesmscodepasswordrecovery_exception);
            }

        }

        public async Task<Response> UpdateWebdoxRestration(string jsonresponse)
        {
            var jsonObject = JObject.Parse(jsonresponse);

            var idwebdox = Convert.ToString(jsonObject["id_correlative"]);
            _logger.LogInformation($"correlative {idwebdox}");

            var responseresult = await this._webdoxRequestService.WebdoxReponse(idwebdox, jsonresponse);
            _logger.LogInformation($"update record: {responseresult.Message}");

            if (responseresult.Success == false)
            {
                this._logger.LogError($"updated not wass syccesfully {responseresult.Result}");
                return new Response(false, $"Se ha recibido respuesta webdox {idwebdox} pero se ha generado un error");
            }

            var iduser = Convert.ToInt32(responseresult.Result);
            _logger.LogInformation($"id user {iduser}");

            var result = await this._webdoxRequestService.AddNewRequestNDA(2, iduser, idwebdox, jsonresponse, "Respuesta de webhook webdox", true);
            _logger.LogInformation($"add new webhook register {result.Message}");

            return new Response(true, $"Se ha recibido respuesta webdox {idwebdox}");
        }

        private async Task StartWedoxFlow(int iduser)
        {
            var userobject = this._userRepository.InstanceObject();
            userobject.ID = iduser;

            var userdata = this._userRepository.UserInfoWebdox(userobject);

            if (userdata.Keys.Count <= 0)
            {
                //return new Response(false, "informancion no valida");
                return;
            }
            var parameters = this._parametersRepository.GetParameters("WEBDOXSERVICES");
            //var webdox = new NdaContract(this._configuration.GetValue<string>("WebdoxServices:Credentials:UsernameService"),
            //this._configuration.GetValue<string>("WebdoxServices:Credentials:PasswordService"),
            //this._configuration.GetValue<string>("WebdoxServices:Credentials:GranttypeService"),
            //this._configuration.GetValue<string>("WebdoxServices:Credentials:Customer"));
            var webdox = new NdaContract(parameters["CREDENTIALS:USERNAMESERVICE"].ToString(),
                                         parameters["CREDENTIALS:PASSWORDSERVICE"].ToString(),
                                         parameters["CREDENTIALS:GRANTTYPESERVICE"].ToString(),
                                         parameters["CREDENTIALS:CUSTOMER"].ToString(),
                                         parameters["BASE"].ToString());

            var auth = await webdox.Authentication(parameters["LOGIN"].ToString());
            if (auth)
            {
                _logger.LogInformation("Login seccess with webdox");
                var data = webdox.GetFormRequest((string)userdata["FULLNAME"], parameters["DEFAULTWORKFLOWDESCRIPTION"].ToString(), parameters["DEFAULTWORKFLOWNAME"].ToString(), parameters["FORMIDNDA"].ToString());

                var contractdata = webdox.GetContractDataIntance();

                contractdata.Fullname = (string)userdata["FULLNAME"];
                contractdata.EntityBirth = (string)userdata["ENTITYBIRTH"];
                contractdata.Birthday = Convert.ToDateTime(userdata["BIRTHDAY"]).ToShortDateString();
                contractdata.Occupation = (string)userdata["OCCUPATION"];
                contractdata.ExpeditionPlace = (string)userdata["EXPEDITIONPLACE"];
                contractdata.Rfc = (string)userdata["RFC"];
                contractdata.Address1 = (string)userdata["ADDRESS1"];
                //contractdata.Address2 = (string)userdata["ADDRESS1"];
                contractdata.Cellphone = (string)userdata["CELLPHONE"];
                contractdata.Email = (string)userdata["EMAIL"];
                contractdata.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");

                var contractkeys = webdox.GetContractDataFormated(contractdata, parameters);
                data.request_form_attribute = contractkeys;

                var newrequest = await webdox.NewRequest(data, parameters["NEWREQUEST"].ToString());


                if (!newrequest.Success)
                {
                    this._logger.LogInformation(newrequest.Message);
                    throw new Exception("Error, en el flujo de webdox");
                }
                var newrequestresult = newrequest.Result as Dictionary<string, object>;
                _logger.LogInformation("New request success");
                _logger.LogInformation(newrequest.Message);

                var newrequestsaved = await this._webdoxRequestService.AddNewRequestNDA(1, iduser, Convert.ToString(newrequestresult["id"]), newrequest.Message, Convert.ToString(newrequestresult["description"]));

                _logger.LogInformation(newrequestsaved.Message);
                _logger.LogInformation("request data is saved");

                var externalsigner = webdox.GetSignerIntance();
                externalsigner.email = (string)userdata["EMAIL"];
                externalsigner.name = (string)userdata["FULLNAME"];
                externalsigner.national_identification_number = (string)userdata["DOCUMENTNUMBER"];

                var signerservice = await webdox.AddSigner(externalsigner, parameters["SIGNER"].ToString(), parameters["STEPSIGNER"].ToString());
                _logger.LogInformation("signer was added");
                _logger.LogInformation(signerservice.Message);

                var comments = webdox.GetInitiateSignersIntance();

                var startservice = await webdox.InitiateSignatures(comments, parameters["STARTCELEBRATION"].ToString(), parameters["STEPSIGNER"].ToString());

                try
                {
                    var userInstance = this._userRepository.InstanceObject();
                    userInstance.CELLPHONE = userdata["CELLPHONE"].ToString();
                    var userstored = this._userRepository.FindByCellphone(userInstance);
                    userstored.PROCESSCONTRACT = DateTime.Now;
                    this._userRepository.Update(userstored);
                    var saved = this._userRepository.Save();

                    _logger.LogInformation("Resultado de guardado base de datos");
                    _logger.LogInformation(saved.Message);
                    _logger.LogInformation("parameter: " + userInstance.CELLPHONE);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }

                _logger.LogInformation("workflow webdox was create succesly");
                _logger.LogInformation(startservice.Message);

                //return new Response(true, "se ha generado correctamente la peticion de contrato");
            }

            //return new Response(false, "Error al iniciar webdox");
        }
    }

}