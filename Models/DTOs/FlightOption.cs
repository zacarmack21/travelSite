using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightOption
    {
        [JsonProperty("flights")]
        [JsonPropertyName("flights")]
        public List<FlightSegment>? Flights { get; set; }

        [JsonProperty("layovers")]
        [JsonPropertyName("layovers")]
        public List<Layover>? Layovers { get; set; }

        [JsonProperty("total_duration")]
        [JsonPropertyName("total_duration")]
        public int TotalDuration { get; set; } // In minutes

        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; } // Assuming decimal for currency

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public required string Type { get; set; } // e.g., "Round trip"

        [JsonProperty("carbon_emissions")]
        [JsonPropertyName("carbon_emissions")]
        public CarbonEmissions? CarbonEmissions { get; set; }

        [JsonProperty("airline_logo")]
        [JsonPropertyName("airline_logo")]
        public string? AirlineLogo { get; set; }

        [JsonProperty("departure_token")]
        [JsonPropertyName("departure_token")]
        public string? DepartureToken { get; set; }

        [JsonProperty("booking_token")]
        [JsonPropertyName("booking_token")]
        public string? BookingToken { get; set; }
        // Note: The API doc also shows airline_logo and extensions at this level,
        // decide if they are needed based on frontend requirements.
    }
} 