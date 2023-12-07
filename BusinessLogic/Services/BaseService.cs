using Azure.Storage.Blobs;
using DataSource;
using DataSource.Contracts;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Utilities;

namespace BusinessLogic.Services
{
    internal class BaseService
    {
        protected readonly AppDbContext _context;
        protected readonly TextInfo _textInfo;
        protected IParametersRepository _parametersRepository;
        private readonly IConfiguration _configuration;
        protected BlobContainerClient _containerClient;

        public BaseService(AppDbContext context, IParametersRepository parametersRepository)
        {
            this._context = context;
            this._textInfo = new CultureInfo("es-MX", false).TextInfo;
            this._parametersRepository = parametersRepository;
        }

        public BaseService(AppDbContext context)
        {
            this._context = context;
            this._textInfo = new CultureInfo("es-MX", false).TextInfo;
        }

        public BaseService(IConfiguration configuration)
        {
            this._textInfo = new CultureInfo("es-MX", false).TextInfo;
            this._configuration = configuration;
        }

        //public BaseService(IConfiguration configuration, IParametersRepository parametersRepository)
        //{
        //    this._textInfo = new CultureInfo("es-MX", false).TextInfo;
        //    this._configuration = configuration;
        //    this._parametersRepository = parametersRepository;
        //}

        protected string ToTitle(string param)
        {
            return _textInfo.ToTitleCase(param.ToLower());
        }

        protected virtual void EmailConfirmation(IEnumerable<string> toemails, IEnumerable<string> ccemails, string subject, string fromemail, string url, bool htmlBody, Dictionary<string, object> data)
        {
            var parametersmail = this._parametersRepository.GetParameters("GMAILSETTINGS");

            //string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("GmailSettings:FileNameSettings"));
            string fileSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, parametersmail["FILENAMESETTINGS"].ToString());
            //string appName = _configuration.GetValue<string>("GmailSettings:AppName");
            string appName = parametersmail["APPNAME"].ToString();
            //string accountSender = _configuration.GetValue<string>("GmailSettings:AccountSender");
            string accountSender = parametersmail["ACCOUNTSENDER"].ToString();

            var service = GmailApi.CreateService(fileSettings, appName, accountSender);

            if (service != null)
            {
                string template = CreateEmailConfimationBodyMessage(data, url);
                string logo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
                var mailbase = GmailApi.CreateMailBase(toemails, ccemails, subject, fromemail, htmlBody);
                var view = GmailApi.CreateAlterview(template);
                var linklogo = GmailApi.CreateLinkResouse(logo, "logo");
                view.LinkedResources.Add(linklogo);
                var mail = GmailApi.AddAlterviewToMail(mailbase, view);
                var googlemail = GmailApi.CreateGoogleMail(mail);

                var response = service.Users.Messages.Send(googlemail, "me").Execute();
            }
        }

        protected virtual string CreateEmailConfimationBodyMessage(Dictionary<string, object> data, string url)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/logo.png");
            var logo = File.ReadAllBytes(logoPath);
            //string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configuration.GetValue<string>("EmailConfirmation:EmailTemplateName"));
            string emailTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this._parametersRepository.GetParameter<string>("EMAILCONFIRMATION", "EMAILTEMPLATENAME"));
            string template = ReadFile.ReadAll(emailTemplatePath);
            //template = template.Replace("##PRICE##", Convert.ToDouble(data["Price"]).ToString("C", CultureInfo.CurrentCulture));
            //template = template.Replace("##DESCRIPTION##", (string)data["Description"]);
            template = template.Replace("##URL##", url);

            return template;
        }

        protected string DecodeString(string encryptString, string privateKeyPath)
        {
            var rsaProvider = RSA.Create();
            var key = File.ReadAllText(privateKeyPath);
            rsaProvider.ImportFromPem(key);
            var bytes = rsaProvider.Decrypt(Convert.FromBase64String(encryptString), RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(bytes);
        }

        protected string PrivateKeyFilePath(string nameFile)
        {
            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nameFile);

            return fullpath;
        }
    }
}
