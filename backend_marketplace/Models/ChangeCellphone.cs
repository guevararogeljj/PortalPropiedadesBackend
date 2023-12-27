using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class ChangeCellphone : BaseModel
    {
        //[Required]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]
        //[EmailAddress]
        //[JsonPropertyName("email")]
        //public string Email { get; set; }

        [Required]
        [JsonPropertyName("cellphone")]
        [StringLength(10, ErrorMessage = "Celular demasiado largo, 10 caracteres maximo")]
        [RegularExpression(@"^([0-9]{10})$")]
        public string Cellphone { get; set; }

        [Required]
        [JsonPropertyName("newcellphone")]
        [RegularExpression(@"^([0-9]{10})$")]
        [StringLength(10, ErrorMessage = "Celular demasiado largo, 10 caracteres maximo")]
        public string NewCellphone { get; set; }

        public override string ToString()
        {
            return $"Cellphone: {this.Cellphone} - NewCellphone: {this.NewCellphone}";
        }
    }
}
