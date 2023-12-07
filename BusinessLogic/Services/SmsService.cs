using BusinessLogic.Contracts;
using DataSource.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities;

namespace BusinessLogic.Services
{
    internal class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        private readonly IParametersRepository _parametersRepository;
        private readonly ILogger<SmsService> _logger;

        public SmsService(IConfiguration configuration, IParametersRepository parametersRepository, ILogger<SmsService> logger)
        {
            this._configuration = configuration;
            this._parametersRepository = parametersRepository;
            this._logger = logger;
        }

        public async Task<Dictionary<string, object>> SendSmsOtp(string cellphone)
        {
            var dic = new Dictionary<string, object>();
            var parameters = this._parametersRepository.GetParameters("OTPSERVICES");
            dic.Add("method", parameters["METHODSENDSMSOTP"].ToString());
            dic.Add("action", parameters["ACTIONSENDSMSOTP"].ToString());



            var body = UrlRequests.CreateSmsBodyRequestJson(cellphone, parameters["MESSAGE"].ToString(), parameters["SENDERSENDSMSOTP"].ToString(), Convert.ToInt32(parameters["DIGITSNUMBER"]));
            var response = await UrlRequests.PostApiJsonRequest(parameters["MESSENGINGSERVICE"].ToString(), body, dic);
            dic.Add("response", response);

            this._logger.LogInformation(string.Format(StringResources.smsservice_sendsmsotp_success, response));

            return dic;

        }

        public async Task<Dictionary<string, object>> ValidateSmsOtp(string cellphone, string code)
        {
            var headers = new Dictionary<string, object>();
            var parameters = this._parametersRepository.GetParameters("OTPSERVICES");
            headers.Add("action", parameters["ACTIONVALIDATESMSOTP"].ToString());

            string url = string.Format(parameters["VALIDATIONSERVICE"].ToString(), cellphone, code);
            var response = await UrlRequests.MakeRequest<Dictionary<string, object>>(url, HttpMethod.Post, new Dictionary<string, object>(), headers);
            var resultService = UrlRequests.CreateObjectFromJson<Dictionary<string, object>>(response);

            this._logger.LogInformation(string.Format(StringResources.smsservice_validatesmsotp_success, String.Join(Environment.NewLine, resultService)));

            return resultService;
        }
    }
}
