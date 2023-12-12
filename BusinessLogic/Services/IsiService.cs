using BusinessLogic.Contracts;
using DataSource.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Utilities;

namespace BusinessLogic.Services
{
    internal class IsiService : IIsiService
    {
        private readonly IParametersRepository _parametersRepository;
        private readonly ILogger<AzureBlobService> _logger;
        private string token = string.Empty;
        private Dictionary<string, object> _parameters;
        private readonly Dictionary<string, object> _isiparameters;

        public IsiService(IParametersRepository parametersRepository, ILogger<AzureBlobService> logger)
        {
            this._parametersRepository = parametersRepository;
            this._logger = logger;
            this._parameters = this._parametersRepository.GetParameters("APISEC");
            this._isiparameters = this._parametersRepository.GetParameters("ISISERVICE");
        }



        public async Task<string> GenerateToken()
        {
            string urlservice = _parameters["URL"].ToString();
            string audience = _parameters["AUDIENCIA"].ToString();
            string password = _parameters["CONTRASENIA"].ToString();
            string user = _parameters["USUARIO"].ToString();

            Dictionary<string, object> body = new Dictionary<string, object>() { };
            body.Add("audiencia", audience);
            body.Add("usuario", user);
            body.Add("contrasenia", password);

            string jsonbody = APIHelper.ConvertToJson(body);
            var result = await APIHelper.ConsumeAPIWithBearer<JObject>(urlservice, HttpMethod.Post, jsonbody);

            return result["token"].ToString();

        }

        public async Task<bool> StatusSoldByCredit(string credit, string token, List<string> soldstatus)
        {
            var issold = false;
            string urlservice = _isiparameters["URL"].ToString();
            string fieldcredit = _isiparameters["CREDITO"].ToString();
            string fielddate = _isiparameters["FECHA"].ToString();
            string fieldstatus = _isiparameters["ESTATUSVENDIDO"].ToString(); //ESTATUSVENDIDO

            Dictionary<string, object> body = new Dictionary<string, object>() { };
            body.Add(fieldcredit, credit);
            body.Add(fielddate, DateTime.Now.ToString("yyyyMMdd"));

            string jsonbody = APIHelper.ConvertToJson(body);
            try
            {
                var result = await APIHelper.ConsumeAPIWithBearer<JObject>(urlservice, HttpMethod.Post, jsonbody, null, token);

                if (result.ContainsKey(fieldstatus))
                {
                    var status = result[fieldstatus].ToString();

                    issold = soldstatus.Any(x => x.Equals(status));                    

                    return issold;
                }

                _logger.LogInformation($"Propiedad no encontrada en isi {credit}");
                return issold;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                _logger.LogError(ex.Message);
                return issold;
            }
        }
    }
}
