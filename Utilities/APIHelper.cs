using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Utilities
{
    public static class APIHelper
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<T> ConsumeAPIWithBearer<T>(string url, HttpMethod httpMethod, string jsonBody = null, Dictionary<string, string> headers = null, string bearerToken = null)
        {
            try
            {
                ConfigureHeaders(headers, bearerToken);

                HttpContent content = ConfigureJSONContent(jsonBody);

                HttpResponseMessage response = await SendHTTPRequest(url, httpMethod, content);

                return await ProcessResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<T> ConsumeAPIWithBasicAuthentication<T>(string url, HttpMethod httpMethod, string jsonBody = null, Dictionary<string, string> headers = null, string basicAuthUsername = null, string basicAuthPassword = null)
        {
            try
            {
                ConfigureHeaders(headers, null);

                ConfigureBasicAuthentication(basicAuthUsername, basicAuthPassword);

                HttpContent content = ConfigureJSONContent(jsonBody);

                HttpResponseMessage response = await SendHTTPRequest(url, httpMethod, content);

                return await ProcessResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ConfigureHeaders(Dictionary<string, string> headers, string bearerToken)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(bearerToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }
        }

        private static void ConfigureBasicAuthentication(string basicAuthUsername, string basicAuthPassword)
        {
            if (!string.IsNullOrEmpty(basicAuthUsername) && !string.IsNullOrEmpty(basicAuthPassword))
            {
                var base64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{basicAuthUsername}:{basicAuthPassword}"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64AuthString);
            }
        }

        private static HttpContent ConfigureJSONContent(string jsonBody)
        {
            HttpContent content = null;
            if (!string.IsNullOrEmpty(jsonBody))
            {
                content = new StringContent(jsonBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return content;
        }

        private static async Task<HttpResponseMessage> SendHTTPRequest(string url, HttpMethod httpMethod, HttpContent content)
        {
            HttpResponseMessage response;
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls13;

            switch (httpMethod.Method.ToUpper())
            {
                case "GET":
                    response = await httpClient.GetAsync(url);
                    break;
                case "POST":
                    response = await httpClient.PostAsync(url, content);
                    break;
                case "DELETE":
                    response = await httpClient.DeleteAsync(url);
                    break;
                case "PUT":
                    response = await httpClient.PutAsync(url, content);
                    break;
                default:
                    throw new ArgumentException("Invalid HTTP method");
            }
            return response;
        }

        private static async Task<T> ProcessResponse<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);

            return result;
        }

        public static string ConvertToJson<T>(T obj)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

                return json;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting the object to JSON: " + ex.Message);
                return null;
            }
        }
    }
}
