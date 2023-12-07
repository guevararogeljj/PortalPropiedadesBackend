using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class FindParams
    {
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonPropertyName("items")]
        public int? Items { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("propertytype")]
        public int? PropertyType { get; set; }

        [JsonPropertyName("state")]
        public int? State { get; set; }

        [JsonPropertyName("city")]
        public int? City { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("rooms")]
        public int? Rooms { get; set; }

        [JsonPropertyName("bathrooms")]
        public int? Bathrooms { get; set; }

        [JsonPropertyName("proceduralstage")]
        public int? ProceduralStage { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
