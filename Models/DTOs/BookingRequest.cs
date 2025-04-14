using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingRequest
    {
        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string? Url { get; set; } // URL for the booking action

        [JsonProperty("post_data")]
        [JsonPropertyName("post_data")]
        public string? PostData { get; set; } // Opaque data string
    }
} 