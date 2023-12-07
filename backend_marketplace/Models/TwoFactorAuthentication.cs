using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class TwoFactorAuthentication : BaseModel
    {
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "El correo no cuenta con un formato valido")]
        //[JsonPropertyName("email")]
        //[Required]
        //public string? Email { get; set; }

        [JsonPropertyName("cellphone")]
        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        public string? Cellphone { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "El codigo no es valido")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
