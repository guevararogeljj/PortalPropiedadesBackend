using BusinessLogic.Contracts;
using BusinessLogic.Models;
using DataSource.Contracts;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace BusinessLogic.Services
{
    internal class CrmService : ICrmService
    {
        private readonly IConfiguration _configuration;
        private readonly IParametersRepository _parametersRepository;

        public CrmService(IConfiguration configuration, IParametersRepository parametersRepository)
        {
            this._configuration = configuration;
            this._parametersRepository = parametersRepository;
        }

        public async Task<Response> ClientToCrm(Dictionary<string, object> data)
        {
            string name = (string)data["Fullname"];
            string email = (string)data["Email"];
            string phone = (string)data["Phone"];

            var parameters = this._parametersRepository.GetParameters("CRMSETTINGS");

            string url = parameters["URL"].ToString();
            Dictionary<string, object> databody = new Dictionary<string, object>();
            //databody.Add("form_id", this._configuration.GetValue<string>("CrmSettings:Form_id"));
            databody.Add("form_id", parameters["FORM_ID"].ToString());
            databody.Add("lead_id", this.GenerateId());
            databody.Add("fullname", name);
            databody.Add("phone", phone);
            databody.Add("email", email);
            //databody.Add("medio", this._configuration.GetValue<string>("CrmSettings:Medio"));
            databody.Add("medio", parameters["MEDIO"].ToString());
            //databody.Add("omnicanalidad_key", this._configuration.GetValue<string>("CrmSettings:Omnicanalidad_key"));
            databody.Add("omnicanalidad_key", parameters["OMNICANALIDAD_KEY"].ToString());
            //databody.Add("canal", this._configuration.GetValue<string>("CrmSettings:Canal_Landing"));
            databody.Add("canal", parameters["CANAL_LANDING"].ToString());
            //databody.Add("fuente", this._configuration.GetValue<string>("CrmSettings:Fuente"));
            databody.Add("fuente", parameters["FUENTE"].ToString());

            string body = UrlRequests.CreateBodyRequestJson(databody);

            var response = UrlRequests.PostApiJsonRequest(url, body, new Dictionary<string, object>(), new Dictionary<string, object>());

            return new Response(true, response.Result);
        }

        private string GenerateId()
        {
            string id = string.Format("{0}-{1}-{2}", "PROP", DateTime.Now.ToShortDateString(), Guid.NewGuid().ToString());

            return id;
        }
    }
}
