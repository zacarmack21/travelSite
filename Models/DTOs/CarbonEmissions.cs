using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class CarbonEmissions
    {
        [JsonProperty("this_flight")]
        [JsonPropertyName("this_flight")]
        public int? ThisFlight { get; set; } // In grams

        [JsonProperty("typical_for_this_route")]
        [JsonPropertyName("typical_for_this_route")]
        public int? TypicalForThisRoute { get; set; } // In grams

        [JsonProperty("difference_percent")]
        [JsonPropertyName("difference_percent")]
        public int? DifferencePercent { get; set; }
    }
} 