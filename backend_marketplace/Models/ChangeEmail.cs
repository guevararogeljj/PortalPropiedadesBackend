using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class ChangeEmail : BaseModel
    {
        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$", ErrorMessage = "Formato de correo incorrecto")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email demasiado largo, 50 caracteres maximo")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$", ErrorMessage = "Formato de correo incorrecto")]
        [EmailAddress]
        [JsonPropertyName("newemail")]
        public string NewEmail { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
