using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class File : BaseModel
    {
        [Required]
        [JsonPropertyName("file")]
        public string? FileBase64 { get; set; }
        [Required]
        [JsonPropertyName("filename")]
        public string? FileName { get; set; }
        [JsonPropertyName("size")]
        public double Size { get; set; }

        [JsonPropertyName("filetype")]
        [Required]
        public string? FileType { get; set; }

        [JsonPropertyName("cellphone")]
        [Required]
        public string? Cellphone { get; set; }

    }
}
