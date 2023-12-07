using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class AntiforgeryToken : BaseModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
