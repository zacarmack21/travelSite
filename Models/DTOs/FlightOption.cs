using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightOption
    {
        [JsonPropertyName("flights")]
        public List<FlightSegment>? Flights { get; set; }

        [JsonPropertyName("layovers")]
        public List<Layover>? Layovers { get; set; }

        [JsonPropertyName("total_duration")]
        public int TotalDuration { get; set; } // In minutes

        [JsonPropertyName("price")]
        public decimal Price { get; set; } // Assuming decimal for currency

        [JsonPropertyName("type")]
        public required string Type { get; set; } // e.g., "Round trip"

        [JsonPropertyName("carbon_emissions")]
        public CarbonEmissions? CarbonEmissions { get; set; }

        [JsonPropertyName("departure_token")]
        public string? DepartureToken { get; set; }

        [JsonPropertyName("booking_token")]
        public string? BookingToken { get; set; }
        // Note: The API doc also shows airline_logo and extensions at this level,
        // decide if they are needed based on frontend requirements.
    }
} 