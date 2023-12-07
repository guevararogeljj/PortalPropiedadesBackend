using LeadsInjection.Contracts;
using Utilities;

namespace LeadsInjection.Services
{
    internal class LeadService : ILeadService
    {
        public Dictionary<string, object> ClientInfoModel(string fullname, string phone, string email)
        {
            Dictionary<string, object> infoclient = new Dictionary<string, object>();
            infoclient.Add("Fullname", fullname);
            infoclient.Add("Phone", phone);
            infoclient.Add("Email", email);

            return infoclient;
        }

        public async Task<Dictionary<string, object>> LeadInjection(string enpoint, string formid, string medio, string apikey, string canal, string fuente, Dictionary<string, object> data)
        {
            string name = (string)data["Fullname"];
            string email = (string)data["Email"];
            string phone = (string)data["Phone"];

            string url = enpoint;

            Dictionary<string, object> databody = new Dictionary<string, object>();
            databody.Add("Form_id", formid);
            databody.Add("Lead_id", this.GenerateId());
            databody.Add("fullname", name);
            databody.Add("phone", phone);
            databody.Add("email", email);
            databody.Add("medio", medio);
            databody.Add("omnicanalidad_key", apikey);
            databody.Add("canal", canal);
            databody.Add("fuente", fuente);

            string body = UrlRequests.CreateBodyRequestJson(databody);

            var response = await UrlRequests.PostApiJsonRequest(url, body, new Dictionary<string, object>(), new Dictionary<string, object>());

            return this.Result(true, response, null);
        }

        private string GenerateId()
        {
            string id = string.Format("{0}-{1}-{2}", "PROP", DateTime.Now.ToShortDateString(), Guid.NewGuid().ToString());

            return id;
        }

        private Dictionary<string, object> Result(bool success, object result, string message)
        {
            var response = new Dictionary<string, object>();
            response.Add("success", success);
            response.Add("result", result);
            response.Add("message", message);

            return response;
        }
    }
}
