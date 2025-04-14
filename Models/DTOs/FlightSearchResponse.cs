using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSearchResponse
    {
        [JsonPropertyName("best_flights")]
        public List<FlightOption>? BestFlights { get; set; }

        [JsonPropertyName("other_flights")]
        public List<FlightOption>? OtherFlights { get; set; }

        [JsonPropertyName("price_insights")]
        public PriceInsights? PriceInsights { get; set; }

        [JsonIgnore]
        public string? ErrorMessage { get; set; }
    }
} 