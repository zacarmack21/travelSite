using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class LocalPrice
    {
        [JsonPropertyName("currency")]
        public string? Currency { get; set; } // e.g., "JPY"

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
} 