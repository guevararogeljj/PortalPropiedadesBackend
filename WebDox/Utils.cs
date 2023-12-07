using Newtonsoft.Json;

namespace WebDox
{
    internal class Utils
    {
        public static Dictionary<string, object> JsonToDictionary(Stream stream)
        {
            using (var streanreadr = new StreamReader(stream))
            {
                using (var jsonreader = new JsonTextReader(streanreadr))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    try
                    {
                        return serializer.Deserialize<Dictionary<string, object>>(jsonreader);
                    }
                    catch (Exception ex)
                    {

                        return new Dictionary<string, object>();
                    }
                }
            }
        }
    }
}
