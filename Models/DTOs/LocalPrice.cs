using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class LocalPrice
    {
        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; } // e.g., "JPY"

        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
} 