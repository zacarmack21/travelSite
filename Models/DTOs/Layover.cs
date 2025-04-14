using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class Layover
    {
        [JsonPropertyName("duration")]
        public int Duration { get; set; } // In minutes

        [JsonPropertyName("name")]
        public required string Name { get; set; } // Airport name

        [JsonPropertyName("id")]
        public required string Id { get; set; } // Airport code
        // Note: API doc also includes 'overnight' boolean, add if needed.
    }
} 