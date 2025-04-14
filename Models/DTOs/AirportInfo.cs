using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class AirportInfo
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; } // Airport code (e.g., "AUS")

        [JsonPropertyName("time")]
        public required string Time { get; set; } // Departure/Arrival time (e.g., "2023-10-03 15:10")
    }
} 