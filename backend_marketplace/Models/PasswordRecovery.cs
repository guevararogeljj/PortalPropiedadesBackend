using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class PasswordRecovery : BaseModel
    {
        [Required]
        [JsonPropertyName("newpassword")]
        public string NewPassword { get; set; }

        //[Required]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]
        //[EmailAddress]
        //[JsonPropertyName("email")]
        //public string Email { get; set; }

        [Required]
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("cellphone")]
        [Required]
        //[Phone]
        //[RegularExpression(@"\d{10}$")]
        public string? Cellphone { get; set; }


    }
}
