using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class Logout : BaseModel
    {
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        //[RegularExpression(@"\d{10}$")]
        [Required(ErrorMessage ="Celular requerido")]
        [JsonPropertyName("cellphone")]
        public string? Cellphone { get; set; }
    }
}
