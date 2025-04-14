using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class PriceInsights
    {
        [JsonPropertyName("lowest_price")]
        public decimal LowestPrice { get; set; }

        [JsonPropertyName("price_level")]
        public string? PriceLevel { get; set; } // e.g., "high", "typical"

        [JsonPropertyName("typical_price_range")]
        public List<decimal>? TypicalPriceRange { get; set; } // [low_bound, high_bound]

        [JsonPropertyName("price_history")]
        public List<List<long>>? PriceHistory { get; set; } // [[timestamp, price], ...]
    }
} 