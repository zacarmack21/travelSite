using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BaggagePrices
    {
        [JsonPropertyName("together")]
        public List<string>? Together { get; set; } // List of strings like "1 free carry-on"
    }
} 