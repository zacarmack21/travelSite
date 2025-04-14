using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingRequest
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; } // URL for the booking action

        [JsonPropertyName("post_data")]
        public string? PostData { get; set; } // Opaque data string
    }
} 