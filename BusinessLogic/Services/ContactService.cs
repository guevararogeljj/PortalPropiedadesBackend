using BusinessLogic.Contracts;
using BusinessLogic.Models;
using DataSource.Contracts;
using LeadsInjection.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Utilities;

namespace BusinessLogic.Services
{
    internal class ContactService : IContactService
    {
        private readonly IContactRepository? _contactRepository;
        private readonly IPropertyRepository? _propertyRepository;
        private readonly IAzureBlobService? _azureBlobService;
        private readonly IUserRepository _userRepository;
        private readonly ILeadService _crmService;
        private readonly IParametersRepository _parametersRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IContactRepository contactRepository, IPropertyRepository propertyRepository, IAzureBlobService azureBlobService, IUserRepository userRepository, IConfiguration configuration, ILeadService crmService, IParametersRepository parametersRepository, ILogger<ContactService> logger)
        {
            this._contactRepository = contactRepository;
            this._configuration = configuration;
            this._propertyRepository = propertyRepository;
            this._azureBlobService = azureBlobService;
            this._userRepository = userRepository;
            this._crmService = crmService;
            this._parametersRepository = parametersRepository;
            this._logger = logger;
        }
        public async Task<Response> AddNewContact(Dictionary<string, object> data)
        {
            try
            {
                var newContact = _contactRepository.InstanceObject();
                newContact.CELLPHONE = data["Cellphone"].ToString();
                newContact.EMAIL = data["Email"].ToString();
                newContact.FULLNAME = data["Fullname"].ToString();
                newContact.MESSAGE = data["Message"].ToString();

                this._contactRepository.Insert(newContact);
                var saved = this._contactRepository.Save();

                if (saved.Success)
                {
                    var parameters = this._parametersRepository.GetParameters("CRMSETTINGS");

                    if (
                        //this._configuration.GetValue<bool>("CrmSettings:Active")
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

                        var infoclient = this._crmService.ClientInfoModel(newContact.FULLNAME, newContact.CELLPHONE, newContact.EMAIL);

                        var crmresutl = await this._crmService.LeadInjection(endpoint, formid, medio, apikey, canal, fuente, infoclient);

                        this._logger.LogInformation($"Crm client send it {newContact.EMAIL}");
                    }

                    //var toemails = _configuration.GetSection("Contact:ToEmails").Get<IEnumerable<string>>();
                    var toemails = this._parametersRepository.GetParameters<string>("CONTACT", "TOEMAILS");
                    //var ccemails = _configuration.GetSection("Contact:CcEmails").Get<IEnumerable<string>>();
                    var ccemails = this._parametersRepository.GetParameters<string>("CONTACT", "CCEMAILS");
                    //var subject = _configuration.GetValue<string>("Contact:Subject");
                    var subject = this._parametersRepository.GetParameter<string>("CONTACT", "SUBJECT");
                    //var fromemail = _configuration.GetValue<string>("Contact:FromEmail");
                    var fromemail = this._parametersRepository.GetParameter<string>("CONTACT", "FROMEMAIL");

                    this.SendEmail(toemails, new List<string>(), subject, fromemail, CreateContactBodyMeesage(data), false);

                    return new Response(true, "Te contactaremos en breve.");
                }
                this._logger.LogInformation($"Ha ocurrido un error al guardar la solicitud de contacto: {saved.Message}", saved.Message);
                return new Response(false, "Ha ocurrido un error al genera su solicitud");
            }
            catch (Exception ex)
            {
                this._logger.LogError("Ha ocurrido un error al tratar de generar una nueva solicitud de contacto" + ex.Message, ex);
                return new Response(false, "Ha pasado un error, por favor vuelve a intentarlo.");
            }
        }

        public async void ShareProperty(Dictionary<string, object> data)
        {
            try
            {
                List<string> toemails = new List<string>();
                toemails.Add((string)data["Email"]);
                var parameters = this._parametersRepository.GetParameters("SHAREOPTIONS");
                //var ccemails = _configuration.GetSection("Contact:CcEmails").Get<IEnumerable<string>>();

                //var ccemails = this._parametersRepository.GetParameter<string>("SHAREOPTIONS", "CCEMAILS");

                //var subject = _configuration.GetValue<string>("ShareOptions:Subject");
                var subject = parameters["SUBJECT"].ToString();
                //var fromemail = _configuration.GetValue<string>("ShareOptions:FromEmail");
                var fromemail = parameters["FROMEMAIL"].ToString();
                //var url = _configuration.GetValue<string>("ShareOptions:BaseUrlProperty");
                var url = parameters["BASEURLPROPERTY"].ToString();

                var propertyRaw = _propertyRepository.SharePropertyInfo(data["CreditNumber"].ToString());

                var CONTAINERNAMEIMAGES = this._parametersRepository.GetParameter<string>("BLOBSTORAGE", "CONTAINERNAMEIMAGES");

                var image = await _azureBlobService.ImageByPath((string)propertyRaw["ImagePath"], CONTAINERNAMEIMAGES);


                foreach (var item in image)
                {
                    data.Add(item.Key, item.Value);
                }

                foreach (var item in propertyRaw)
                {
                    data.Add(item.Key, item.Value);
                }

                this.SendEmailShared(toemails, null, subject, fromemail, url, true, data);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
            }

        }

        private void SendEmail(IEnumerable<string> toemails, IEnumerable<string> ccemails, string subject, string fromemail, string body, bool htmlBody)
        {
            var parameters = this._parametersRepository.GetParameters("GMAILSETTINGS");
            //string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("GmailSettings:FileNameSettings"));
            string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, parameters["FILENAMESETTINGS"].ToString());
            //string appName = _configuration.GetValue<string>("GmailSettings:AppName");
            string appName = parameters["APPNAME"].ToString();
            //string accountSender = _configuration.GetValue<string>("GmailSettings:AccountSender");
            string accountSender = parameters["ACCOUNTSENDER"].ToString();

            var service = GmailApi.CreateService(fileSettings, appName, accountSender);

            if (service != null)
            {
                var email = GmailApi.CreatePackage(toemails, ccemails, subject, fromemail, body, htmlBody);
                var response = service.Users.Messages.Send(email, "me").Execute();
            }
        }

        private void SendEmailShared(IEnumerable<string> toemails, IEnumerable<string> ccemails, string subject, string fromemail, string url, bool htmlBody, Dictionary<string, object> data)
        {
            var parameters = this._parametersRepository.GetParameters("GMAILSETTINGS");
            //string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("GmailSettings:FileNameSettings"));
            string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, parameters["FILENAMESETTINGS"].ToString());
            //string appName = _configuration.GetValue<string>("GmailSettings:AppName");
            string appName = parameters["APPNAME"].ToString();
            //string accountSender = _configuration.GetValue<string>("GmailSettings:AccountSender");
            string accountSender = parameters["ACCOUNTSENDER"].ToString();

            var service = GmailApi.CreateService(fileSettings, appName, accountSender);

            if (service != null)
            {
                using (var stream = new MemoryStream((byte[])data["File"]))
                {
                    string template = CreateShareBodyMessage(data, url);
                    string logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
                    var mailbase = GmailApi.CreateMailBase(toemails, ccemails, subject, fromemail, htmlBody);
                    var view = GmailApi.CreateAlterview(template);
                    var linkhouse = GmailApi.CreateLinkResouse(stream, "house");
                    view.LinkedResources.Add(linkhouse);
                    var linklogo = GmailApi.CreateLinkResouse(logo, "logo");
                    view.LinkedResources.Add(linklogo);
                    var mail = GmailApi.AddAlterviewToMail(mailbase, view);
                    var googlemail = GmailApi.CreateGoogleMail(mail);

                    var response = service.Users.Messages.Send(googlemail, "me").Execute();
                }
            }
        }

        private string CreateContactBodyMeesage(Dictionary<string, object> data)
        {
            //string template = _configuration.GetValue<string>("Contact:BodyEmail");
            string template = this._parametersRepository.GetParameter<string>("CONTACT", "BODYEMAIL");

            string body = string.Format(template, (string)data["Fullname"], (string)data["Message"], (string)data["Email"], (string)data["Cellphone"]);

            return body;
        }

        private string CreateShareBodyMessage(Dictionary<string, object> data, string url)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
            var logo = File.ReadAllBytes(logoPath);
            //string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("ShareOptions:EmailTemplateName"));
            string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._parametersRepository.GetParameter<string>("SHAREOPTIONS", "EMAILTEMPLATENAME"));
            string template = ReadFile.ReadAll(emailTemplatePath);
            template = template.Replace("##PRICE##", Convert.ToDouble(data["Price"]).ToString("C", CultureInfo.CurrentCulture));
            template = template.Replace("##DESCRIPTION##", (string)data["Description"]);
            template = template.Replace("##URL##", string.Format(url, data["CreditNumber"].ToString()));
            template = template.Replace("##TITLE##", data["Title"].ToString());

            return template;
        }

        public async void RequestInfoProperty(Dictionary<string, object> data)
        {
            try
            {
                string email = (string)data["Email"]; //cellphone
                //var toemails = _configuration.GetSection("PropertyInterestOptions:ToEmails").Get<IEnumerable<string>>();
                var toemails = this._parametersRepository.GetParameters<string>("PROPERTYINTERESTOPTIONS", "TOEMAILS");

                var instance = _userRepository.InstanceObject();
                instance.CELLPHONE = email;
                var contactUser = _userRepository.UserInformation(instance);

                //var subject = _configuration.GetValue<string>("PropertyInterestOptions:Subject");
                var subject = this._parametersRepository.GetParameter<string>("PROPERTYINTERESTOPTIONS", "SUBJECT");

                //var fromemail = _configuration.GetValue<string>("PropertyInterestOptions:FromEmail");
                var fromemail = this._parametersRepository.GetParameter<string>("PROPERTYINTERESTOPTIONS", "FROMEMAIL");

                //var url = _configuration.GetValue<string>("PropertyInterestOptions:BaseUrlProperty");
                var url = this._parametersRepository.GetParameter<string>("PROPERTYINTERESTOPTIONS", "BASEURLPROPERTY");

                var propertyRaw = _propertyRepository.SharePropertyInfo(data["CreditNumber"].ToString());

                var CONTAINERNAMEIMAGES = this._parametersRepository.GetParameter<string>("BLOBSTORAGE", "CONTAINERNAMEIMAGES");

                var image = await _azureBlobService.ImageByPath((string)propertyRaw["ImagePath"], CONTAINERNAMEIMAGES);


                foreach (var item in image)
                {
                    data.Add(item.Key, item.Value);
                }

                foreach (var item in propertyRaw)
                {
                    data.Add(item.Key, item.Value);
                }

                foreach (var item in contactUser)
                {
                    data.Add(item.Key, item.Value);
                }

                this.SendRequestEmail(toemails, null, subject, fromemail, url, true, data);
                this._logger.LogInformation("Se envia notificacion a inmuebles de usuario solicitando informacion de propiedad");
            }
            catch (Exception ex)
            {
                this._logger.LogError("Error al enviar notificacion a inmuebles de usuario solicitando informacion de propiedad", ex);
                //return new Response(false, "Ha pasado un error, por favor vuelve a intentarlo.");
            }

        }

        private void SendRequestEmail(IEnumerable<string> toemails, IEnumerable<string> ccemails, string subject, string fromemail, string url, bool htmlBody, Dictionary<string, object> data)
        {
            var parameters = this._parametersRepository.GetParameters("GMAILSETTINGS");
            //string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("GmailSettings:FileNameSettings"));
            string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, parameters["FILENAMESETTINGS"].ToString());
            //string appName = _configuration.GetValue<string>("GmailSettings:AppName");
            string appName = parameters["APPNAME"].ToString();
            //string accountSender = _configuration.GetValue<string>("GmailSettings:AccountSender");
            string accountSender = parameters["ACCOUNTSENDER"].ToString();

            var service = GmailApi.CreateService(fileSettings, appName, accountSender);

            if (service != null)
            {
                using (var stream = new MemoryStream((byte[])data["File"]))
                {
                    string template = CreateRequestEmailTemplate(data, url);
                    string logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
                    var mailbase = GmailApi.CreateMailBase(toemails, ccemails, subject, fromemail, htmlBody);
                    var view = GmailApi.CreateAlterview(template);
                    var linkhouse = GmailApi.CreateLinkResouse(stream, "house");
                    view.LinkedResources.Add(linkhouse);
                    var linklogo = GmailApi.CreateLinkResouse(logo, "logo");
                    view.LinkedResources.Add(linklogo);
                    var mail = GmailApi.AddAlterviewToMail(mailbase, view);
                    var googlemail = GmailApi.CreateGoogleMail(mail);

                    var response = service.Users.Messages.Send(googlemail, "me").Execute();
                }
            }
        }

        private string CreateRequestEmailTemplate(Dictionary<string, object> data, string url)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
            var logo = File.ReadAllBytes(logoPath);
            //string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("PropertyInterestOptions:EmailTemplateName"));
            string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._parametersRepository.GetParameter<string>("PROPERTYINTERESTOPTIONS", "EMAILTEMPLATENAME"));
            string template = ReadFile.ReadAll(emailTemplatePath);
            //template = template.Replace("##PRICE##", Convert.ToDouble(data["Price"]).ToString("C", CultureInfo.CurrentCulture));
            template = template.Replace("##DESCRIPTION##", (string)data["Description"]);
            template = template.Replace("##URL##", string.Format(url, data["CreditNumber"].ToString()));

            template = template.Replace("##CREDITNUMBER##", (string)data["Credit"].ToString());

            template = template.Replace("##NAME##", (string)data["name"].ToString());
            template = template.Replace("##CELLPHONE##", (string)data["cellphone"].ToString());
            template = template.Replace("##EMAIL##", (string)data["email"].ToString());

            return template;
        }


    }
}
