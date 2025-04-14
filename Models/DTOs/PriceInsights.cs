using System.Collections.Generic;

namespace TravelSite.Models.DTOs
{
    public class PriceInsights
    {
        public decimal LowestPrice { get; set; }
        public string? PriceLevel { get; set; } // e.g., "high", "typical"
        public List<decimal>? TypicalPriceRange { get; set; } // [low_bound, high_bound]
        public List<List<long>>? PriceHistory { get; set; } // [[timestamp, price], ...]
    }
} 