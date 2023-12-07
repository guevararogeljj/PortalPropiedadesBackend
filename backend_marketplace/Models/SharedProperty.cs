using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class SharedProperty : BaseModel
    {

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-z0-9-ZñÑáéíóúÁÉÍÓÚ\s]+")]
        [JsonPropertyName("creditnumber")]
        public string? CreditNumber { get; set; }
    }
}
