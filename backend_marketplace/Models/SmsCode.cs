using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class SmsCode : BaseModel
    {
        [JsonPropertyName("code")]
        [RegularExpression(@"^([0-9]{4})$")]
        public string? Code { get; set; }
        //[RegularExpression(@"\d{10}$")]
        [StringLength(10, ErrorMessage = "Celular demasiado largo, 10 caracteres maximo")]
        [JsonPropertyName("cellphone")]
        public string? Cellphone { get; set; }
    }
}
