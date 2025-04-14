using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BaggagePrices
    {
        [JsonProperty("together")]
        [JsonPropertyName("together")]
        public List<string>? Together { get; set; } // List of strings like "1 free carry-on"
    }
} 