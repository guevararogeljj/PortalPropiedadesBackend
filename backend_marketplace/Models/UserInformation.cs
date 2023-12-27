using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class UserInformation : BaseModel
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("names")]
        [Required]
        [StringLength(50, ErrorMessage = "Nombre demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Names { get; set; }

        [JsonPropertyName("lastname")]
        [Required]
        [StringLength(50, ErrorMessage = "Apellido demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Lastname { get; set; }

        [JsonPropertyName("lastname2")]
        [Required]
        [StringLength(50, ErrorMessage = "Apellido demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Lastname2 { get; set; }

        [JsonPropertyName("gender")]
        [Required]
        [StringLength(6)]
        public string? Gender { get; set; }

        [JsonPropertyName("birthday")]
        [Required]
        [DataType(DataType.Date)]
        public string? Birthday { get; set; }

        [JsonPropertyName("birthplace")]
        [Required]
        [StringLength(50)]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Birthplace { get; set; }

        [JsonPropertyName("terms")]
        [Required, Range(typeof(bool), "true", "false")]
        public string? Terms { get; set; }


    }
}
