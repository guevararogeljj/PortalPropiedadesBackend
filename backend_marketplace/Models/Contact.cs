using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class Contact : BaseModel
    {
        private string? fullname;
        private string? cellphone;
        private string? email;
        private string? message;

        [Required]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z a-zÑñ\s]+$")]
        [JsonPropertyName("fullname")]
        public string? Fullname { get => fullname.Trim(); set => fullname = value; }

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get => email; set => email = value; }

        [Required]
        [RegularExpression(@"^[0-9]*$")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(10)]
        [JsonPropertyName("cellphone")]
        public string? Cellphone { get => cellphone.Trim(); set => cellphone = value; }

        [Required]
        [RegularExpression(@"[a-zA-z0-9-ZñÑáéíóúÁÉÍÓÚ\s]+")]
        [StringLength(500, MinimumLength = 8)]
        [JsonPropertyName("message")]
        public string? Message { get => message; set => message = value; }

        [JsonPropertyName("terms")]
        [Required]
        public Boolean Terms { get; set; }
    }
}
