
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Utilities
{
    public static class UrlRequests
    {
        public static async Task<string> PostApiJsonRequest(string url, string body, Dictionary<string, object> headers = null, Dictionary<string, object> authorization = null)
        {
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

            foreach (var item in headers)
            {
                content.Headers.Add(item.Key, item.Value.ToString());
            }

            try
            {
                using (var client = new HttpClient())
                {
                    if (authorization != null && authorization.Count > 0)
                    {
                        client.DefaultRequestHeaders.Add(authorization.FirstOrDefault().Key, authorization[authorization.FirstOrDefault().Key].ToString());
                    }
                    using (var response = await client.PostAsync(url, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }

                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public static async Task<string> PostApiJsonRequest(string url, Dictionary<string, object> headers = null)
        {
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");

            foreach (var item in headers)
            {
                content.Headers.Add(item.Key, item.Value.ToString());
            }

            try
            {
                using (var client = new HttpClient())
                {

                    using (var response = await client.PostAsync(url, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }

                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public static async Task<string> GetApiJsonRequest(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }

                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public static string CreateSmsBodyRequestJson(string cellphone, string message, string sender, int digits)
        {
            var body = new { phone = cellphone, msg = message, length = digits, sender = sender };

            string jsonBody = JsonSerializer.Serialize(body);

            return jsonBody;
        }

        public static string CreateBodyRequestJson(Dictionary<string, object> body)
        {
            string jsonBody = JsonSerializer.Serialize(body);

            return jsonBody;
        }

        public static string CreateSmsBodyValidationJson(string cellphone, string code)
        {
            var body = new { phone = cellphone, otp = code };

            string jsonBody = JsonSerializer.Serialize(body);

            return jsonBody;
        }

        public static T CreateObjectFromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static string CreateJsonFromObject<T>(T data)
        {
            return JsonSerializer.Serialize<T>(data);
        }

        public static async Task<string> MakeRequest<T>(string url, HttpMethod method, T jsonbody, Dictionary<string, object> headers = null, AuthenticationHeaderValue auth = null)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(url);

                request.Method = method;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var key in headers)
                    {
                        request.Headers.Add(key.Key, key.Value.ToString());
                    }
                }

                if (auth != null)
                {
                    request.Headers.Authorization = auth;
                }

                if (jsonbody != null)
                {
                    HttpContent content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonbody));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    request.Content = content;

                }

                using (var response = await client.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return null;
        }
    }

}
