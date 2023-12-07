using System.Text.Json.Serialization;

namespace BusinessLogic.Models
{
    public class PropertiesPaged
    {
        [JsonPropertyName("items")]
        public List<PropertyPaged> Items { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("pagesize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

    }

    public class PropertyPaged
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("creditnumber")]
        public int? CreditNumber { get; set; }

        [JsonPropertyName("rooms")]
        public string Rooms { get; set; }

        [JsonPropertyName("bathrooms")]
        public string Bathrooms { get; set; }

        [JsonPropertyName("constructionsize")]
        public double? ConstructionSize { get; set; }

        [JsonPropertyName("lotsize")]
        public double? LotSize { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("parkingspaces")]
        public string ParkingSpaces { get; set; }

        [JsonPropertyName("settlement")]
        public string Settlement { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("favorite")]
        public bool Favorite { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("sold")]
        public bool? Sold { get; set; }

        [JsonIgnore]
        public string Id { get; set; }

    }
}
