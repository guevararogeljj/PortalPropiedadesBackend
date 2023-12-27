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
        [RegularExpression(@"^([0-9]{10})$")]
        public string Cellphone { get; set; }

        [Required]
        [JsonPropertyName("newcellphone")]
        [RegularExpression(@"^([0-9]{10})$")]
        public string NewCellphone { get; set; }

        public override string ToString()
        {
            return $"Cellphone: {this.Cellphone} - NewCellphone: {this.NewCellphone}";
        }
    }
}
