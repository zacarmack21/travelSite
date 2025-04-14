using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class AirportInfo
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public required string Id { get; set; } // Airport code (e.g., "AUS")

        [JsonProperty("time")]
        [JsonPropertyName("time")]
        public required string Time { get; set; } // Departure/Arrival time (e.g., "2023-10-03 15:10")
    }
} 