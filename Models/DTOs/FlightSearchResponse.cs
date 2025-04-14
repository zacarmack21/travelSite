using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSearchResponse
    {
        [JsonProperty("best_flights")]
        [JsonPropertyName("best_flights")]
        public List<FlightOption>? BestFlights { get; set; }

        [JsonProperty("other_flights")]
        [JsonPropertyName("other_flights")]
        public List<FlightOption>? OtherFlights { get; set; }

        [JsonProperty("price_insights")]
        [JsonPropertyName("price_insights")]
        public PriceInsights? PriceInsights { get; set; }

        [JsonProperty("error_message")]
        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }
    }
} 