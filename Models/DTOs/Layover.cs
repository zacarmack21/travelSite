using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class Layover
    {
        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; } // In minutes

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public required string Name { get; set; } // Airport name

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public required string Id { get; set; } // Airport code
        // Note: API doc also includes 'overnight' boolean, add if needed.
        // [JsonProperty("overnight")]
        // [JsonPropertyName("overnight")]
        // public bool? Overnight { get; set; }
    }
} 