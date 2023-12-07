using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class Login : BaseModel
    {
        //[Required]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        //public string? Email { get; set; }

        [JsonPropertyName("cellphone")]
        [Required]
        //[Phone]
        //[RegularExpression(@"\d{10}$")]
        public string? Cellphone { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
