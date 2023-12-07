using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class Property : BaseModel
    {
        [JsonPropertyName("creditnumber")]
        public string CreditNumber { get; set; }
    }
}
