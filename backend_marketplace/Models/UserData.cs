using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
	public class UserData : BaseModel
    {
        private string? cellphone;

        [JsonPropertyName("names")]
        [Required]
        [StringLength(50)]

        public string? Names { get; set; } = string.Empty;

        [JsonPropertyName("lastname")]
        [Required]
        [StringLength(50)]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Lastname { get; set; } = string.Empty;

        [JsonPropertyName("lastname2")]
        [Required]
        [StringLength(50)]
        [RegularExpression(@"[a-zñA-ZÑ]+$")]
        public string? Lastname2 { get; set; } = string.Empty;

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        [JsonPropertyName("email")]
        public string? Email { get; set; } = string.Empty;

        [JsonPropertyName("cellphone")]
        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        public string? Cellphone { get => cellphone!.Trim(); set => cellphone = value; }
    }
}

