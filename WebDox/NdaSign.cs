using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using WebDox.Contracts;
using WebDox.Models;

namespace WebDox
{
    public class NdaSign : INdaSign
    {
        private string _username;
        private string _password;
        private string _granttype;
        private HttpClient _client;
        private IConfiguration _configuration;
        private string token;
        private string refreshToken;
        private string workflowformid;
        private const string JSONAPPLICATION_HEADER = "application/json";
        private const string AUTHORIZATION_HEADER = "Bearer";
        //public HttpResponseMessage ResponseService { get; set; }

        public NdaSign(string username, string password, string granttype, IConfiguration configuration)
        {
            this._username = username;
            this._password = password;
            this._granttype = granttype;
            this._configuration = configuration;

            this._client = new HttpClient();
            this._client.BaseAddress = new Uri(this._configuration.GetValue<string>("WebdoxServices:Base"));


        }

        public async Task<Response> Authentication()
        {

            if (!string.IsNullOrEmpty(this.token))
            {
                return this.FailResult();
            }

            try
            {
                this._client.DefaultRequestHeaders.Clear();
                this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JSONAPPLICATION_HEADER));

                var endpoint = this._configuration.GetValue<string>("WebdoxServices:Login");

                var data = new { grant_type = this._granttype, username = this._username, password = this._password };

                var httpresponse = await MakePostRequestToWebdox(data, endpoint);

                this.token = (string)httpresponse["access_token"];

                this.refreshToken = (string)httpresponse["refresh_token"];

                this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AUTHORIZATION_HEADER, this.token);

                return this.SuccessResult();


            }
            catch (HttpRequestException ex)
            {
                return this.FailResult(ex.Message, ex.StackTrace);
            }

        }

        public async Task<Response> NewRequest(FormRequest data)
        {
            try
            {
                if (string.IsNullOrEmpty(this.token))
                {
                    return this.FailResult("Token no valido");
                }

                var endpoint = this._configuration.GetValue<string>("WebdoxServices:NewRequest");

                var httpresponse = await this.MakePostRequestToWebdox(data, endpoint);

                this.workflowformid = (string)httpresponse["workflow_request"]["id"];

                var success = (bool)httpresponse["success"];
                //this.workflowformid = (string)httpresponse["success"];

                if (success)
                {
                    return this.SuccessResult("", httpresponse);
                }

                return this.FailResult("", httpresponse);
            }
            catch (Exception ex)
            {
                return this.FailResult(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Response> AddSigner(Signer data)
        {
            if (string.IsNullOrEmpty(this.token))
            {
                return this.FailResult("Token no valido");
            }

            try
            {
                var stepnumber = this._configuration.GetValue<string>("WebdoxServices:StepSigner");

                var endpoint = string.Format(this._configuration.GetValue<string>("WebdoxServices:Signer"), this.workflowformid, stepnumber);

                var httpresponse = await MakePostRequestToWebdox(data, endpoint);

                return this.SuccessResult("", httpresponse);
            }
            catch (Exception ex)
            {
                return this.FailResult(ex.Message, ex.StackTrace);
            }
        }


        public async Task<Response> InitiateSignatures(InitiateSigners data)
        {

            if (string.IsNullOrEmpty(this.workflowformid))
            {
                return this.FailResult();
            }

            try
            {
                var stepnumber = this._configuration.GetValue<string>("WebdoxServices:StepSigner");

                var endpoint = string.Format(this._configuration.GetValue<string>("WebdoxServices:StartCelebration"), this.workflowformid, stepnumber);

                var httpresponse = await MakePutRequestToWebdox(data, endpoint);

                return this.SuccessResult("", httpresponse);
            }
            catch (Exception ex)
            {
                return this.FailResult(ex.Message, ex.StackTrace);
            }
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

        private Response SuccessResult(string message = null, object result = null)
        {
            return new Response(true, message, result);
        }

        private Response FailResult(string message = null, object result = null)
        {
            return new Response(false, message, result);
        }
    }
}
