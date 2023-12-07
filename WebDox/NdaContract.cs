using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Resources;
using System.Text;
using WebDox.Models;

namespace WebDox
{
    public class NdaContract
    {
        private string _username;
        private string _password;
        private string _granttype;
        private string _customer;
        private HttpClient _client;
        //private IConfiguration _configuration;
        private string token;
        private string refreshToken;
        private string workflowformid;
        private const string JSONAPPLICATION_HEADER = "application/json";
        private const string AUTHORIZATION_HEADER = "Bearer";
        private ResourceManager _configuration;

        public NdaContract(string username, string password, string granttype, string customer, string baseurl) : this(baseurl)
        {
            this._username = username;
            this._password = password;
            this._granttype = granttype;
            this._customer = customer;
        }

        public NdaContract(string baseurl)
        {
            var enviromentVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(enviromentVar))
            {
                throw new NotImplementedException("implement environment variable QA or Prod");
            }

            //if (enviromentVar == "QA")
            //{
            //    this._configuration = new ResourceManager(typeof(ResourceQA));
            //}

            //if (enviromentVar == "Production")
            //{
            //    this._configuration = new ResourceManager(typeof(ResourcePROD));
            //}

            this._client = new HttpClient();
            this._client.BaseAddress = new Uri(baseurl);
        }

        public async Task<bool> Authentication(string loginurl)
        {

            if (!string.IsNullOrEmpty(this.token))
            {
                return true;
            }

            try
            {
                this._client.DefaultRequestHeaders.Clear();
                this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSONAPPLICATION_HEADER));

                var endpoint = loginurl;

                var data = new { grant_type = this._granttype, username = this._username, password = this._password, customer = this._customer };

                var httpresponse = await MakePostRequestToWebdox(data, endpoint);

                this.token = (string)httpresponse["access_token"];

                this.refreshToken = (string)httpresponse["refresh_token"];

                this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_HEADER, this.token);

                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;

            }
        }

        public async Task<Response> NewRequest(FormRequest data, string newrequesturl)
        {
            if (string.IsNullOrEmpty(this.token))
            {
                return new Response() { Success = false };
            }

            JObject httpresponse = null;

            try
            {


                var dic = new Dictionary<string, object>();
                var endpoint = newrequesturl;

                httpresponse = await this.MakePostRequestToWebdox(data, endpoint);

                this.workflowformid = (string)httpresponse["workflow_request"]["id"];
                dic.Add("id", (int)httpresponse["workflow_request"]["id"]);
                dic.Add("status", (string)httpresponse["workflow_request"]["status"]);
                dic.Add("description", (string)httpresponse["workflow_request"]["description"]);

                //dic.Add("id", (int)httpresponse["workflow_request"]["id"]);
                //dic.Add("id", (int)httpresponse["workflow_request"]["id"]);
                //dic.Add("id", (int)httpresponse["workflow_request"]["id"]);

                var success = (bool)httpresponse["success"];

                return new Response() { Success = success, Result = dic, Message = httpresponse.ToString(Newtonsoft.Json.Formatting.None) };

            }
            catch (Exception ex)
            {

                return new Response() { Success = false, Result = null, Message = ex.Message + "/////" + httpresponse.ToString(Newtonsoft.Json.Formatting.None) };
            }
        }

        public async Task<Response> AddSigner(Signer data, string signerurl, string step)
        {
            if (string.IsNullOrEmpty(this.token))
            {
                return new Response(false);
            }

            var stepnumber = step;

            var endpoint = string.Format(signerurl, this.workflowformid, stepnumber);

            var httpresponse = await MakePostRequestToWebdox(data, endpoint);

            //var success = (bool)httpresponse["success"];

            //return new JObject(new { success = success, result = httpresponse.ToObject<Dictionary<string, object>>() });
            return new Response(true, httpresponse.ToString(Newtonsoft.Json.Formatting.None), httpresponse.ToObject<Dictionary<string, object>>());
        }

        public async Task<Response> InitiateSignatures(InitiateSigners data, string starturl, string step)
        {

            if (string.IsNullOrEmpty(this.workflowformid))
            {
                return new Response(false);
            }

            var stepnumber = step;

            var endpoint = string.Format(starturl, this.workflowformid, stepnumber);

            var httpresponse = await MakePutRequestToWebdox(data, endpoint);

            //var success = Convert.ToString(httpresponse["status"]);

            //return new JObject(new { success = success, result = httpresponse });

            return new Response(true, httpresponse.ToString(Newtonsoft.Json.Formatting.None), httpresponse.ToObject<Dictionary<string, object>>());
        }

        public FormRequest GetFormRequest(string clientname, string description, string workflowname, string formid)
        {
            var form = new FormRequest();
            form.workflow_request.description = description;
            form.workflow_request.title = string.Format(workflowname, clientname);
            form.workflow_request.request_form_id = formid;
            return form;
        }

        public ContractData GetContractDataIntance()
        {
            return new ContractData();
        }

        public Dictionary<string, object> GetContractDataFormated(ContractData data, Dictionary<string, object> fieldnames)
        {
            var dic = new Dictionary<string, object>();

            try
            {

                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:ADDRESS1"].ToString(), data.Address1);
                //dic.Add(this._configuration.GetString("Field.Address2"), data.Address2);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:BIRTHDAY"].ToString(), data.Birthday);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:CELLPHONE"].ToString(), data.Cellphone);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:CURRENTDATE"].ToString(), data.CurrentDate);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:EMAIL"].ToString(), data.Email);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:ENTITYBIRTH"].ToString(), data.EntityBirth);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:EXPEDITIONPLACE"].ToString(), data.ExpeditionPlace);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:FULLNAME"].ToString(), data.Fullname);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:OCCUPATION"].ToString(), data.Occupation);
                dic.Add(fieldnames["WEBDOXFORMNDAFIELDS:RFC"].ToString(), data.Rfc);


            }
            catch (Exception ex)
            {
                return dic;

            }

            return dic;
        }

        public InitiateSigners GetInitiateSignersIntance()
        {
            return new InitiateSigners();
        }

        public Signer GetSignerIntance()
        {
            return new Signer();
        }

        private async Task<JObject> MakePostRequestToWebdox(object data, string formatedendpoint)
        {
            try
            {
                var endpoint = formatedendpoint;

                var jsonbody = JsonConvert.SerializeObject(data);

                StringContent stringContent = new StringContent(jsonbody, Encoding.UTF8, JSONAPPLICATION_HEADER);

                var responseService = await this._client.PostAsync(endpoint, stringContent);

                responseService.EnsureSuccessStatusCode();

                var result = await responseService.Content.ReadAsStringAsync();

                //var httpresponse = Utils.JsonToDictionary(result);
                var httpresponse = JObject.Parse(result);

                return httpresponse;
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        private async Task<JObject> MakePutRequestToWebdox(object data, string formatedenpoint)
        {
            try
            {
                var endpoint = formatedenpoint;

                var jsonbody = JsonConvert.SerializeObject(data);

                StringContent stringContent = new StringContent(jsonbody, Encoding.UTF8, JSONAPPLICATION_HEADER);

                var responseService = await this._client.PutAsync(formatedenpoint, stringContent);

                responseService.EnsureSuccessStatusCode();

                var result = await responseService.Content.ReadAsStringAsync();

                var httpresponse = JObject.Parse(result);

                return httpresponse;

            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
