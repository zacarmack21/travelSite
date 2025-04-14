using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingOptionDetail
    {
        [JsonProperty("together")]
        [JsonPropertyName("together")]
        public BookingTogether? Together { get; set; }
    }
} 