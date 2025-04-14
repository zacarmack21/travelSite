using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class CarbonEmissions
    {
        [JsonPropertyName("this_flight")]
        public int? ThisFlight { get; set; } // In grams

        [JsonPropertyName("typical_for_this_route")]
        public int? TypicalForThisRoute { get; set; } // In grams

        [JsonPropertyName("difference_percent")]
        public int? DifferencePercent { get; set; }
    }
} 