using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingOptionDetail
    {
        [JsonPropertyName("together")]
        public BookingTogether? Together { get; set; }
    }
} 