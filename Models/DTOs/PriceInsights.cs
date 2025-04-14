using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class PriceInsights
    {
        [JsonProperty("lowest_price")]
        [JsonPropertyName("lowest_price")]
        public decimal LowestPrice { get; set; }

        [JsonProperty("price_level")]
        [JsonPropertyName("price_level")]
        public string? PriceLevel { get; set; } // e.g., "high", "typical"

        [JsonProperty("typical_price_range")]
        [JsonPropertyName("typical_price_range")]
        public List<decimal>? TypicalPriceRange { get; set; } // [low_bound, high_bound]

        [JsonProperty("price_history")]
        [JsonPropertyName("price_history")]
        public List<List<long>>? PriceHistory { get; set; } // [[timestamp, price], ...]
    }
} 