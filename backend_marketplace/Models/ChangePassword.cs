using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class ChangePassword : BaseModel
    {
        [Required]
        [JsonPropertyName("newpassword")]
        public string NewPassword { get; set; }

        [JsonPropertyName("oldpassword")]
        public string OldPassword { get; set; }

        [JsonPropertyName("code")]
        [RegularExpression(@"^([0-9]{4})$")]
        public string? Code { get; set; }

        [JsonPropertyName("cellphone")]
        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        public string? Cellphone { get; set; }
    }
}
