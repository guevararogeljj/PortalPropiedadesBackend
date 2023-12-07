using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class UpdateTerms : BaseModel
    {
        [JsonPropertyName("terms")]
        public bool Terms { get; set; }

        //[JsonPropertyName("email")]
        //[Required]
        //[EmailAddress]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        //public string? Email { get; set; }

        [RegularExpression(@"\d{10}$")]
        [JsonPropertyName("cellphone")]
        public string? Cellphone { get; set; }
    }
}
