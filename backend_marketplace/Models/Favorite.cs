using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class Favorite : BaseModel
    {
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        //[JsonPropertyName("email")]
        //public string? Email { get; set; }
        [JsonPropertyName("cellphone")]
        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        public string? Cellphone { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
