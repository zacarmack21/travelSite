using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSegment
    {
        [JsonProperty("departure_airport")]
        [JsonPropertyName("departure_airport")]
        public required AirportInfo DepartureAirport { get; set; }

        [JsonProperty("arrival_airport")]
        [JsonPropertyName("arrival_airport")]
        public required AirportInfo ArrivalAirport { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; } // In minutes

        [JsonProperty("airplane")]
        [JsonPropertyName("airplane")]
        public string? Airplane { get; set; }

        [JsonProperty("airline")]
        [JsonPropertyName("airline")]
        public required string Airline { get; set; }

        [JsonProperty("airline_logo")]
        [JsonPropertyName("airline_logo")]
        public required string AirlineLogo { get; set; } // URL

        [JsonProperty("flight_number")]
        [JsonPropertyName("flight_number")]
        public required string FlightNumber { get; set; }

        [JsonProperty("travel_class")]
        [JsonPropertyName("travel_class")]
        public required string TravelClass { get; set; }

        [JsonProperty("legroom")]
        [JsonPropertyName("legroom")]
        public string? Legroom { get; set; } // e.g., "31 in"

        [JsonProperty("extensions")]
        [JsonPropertyName("extensions")]
        public List<string>? Extensions { get; set; }
        // Note: The API doc also shows airplane, extensions, ticket_also_sold_by, overnight, etc.
        // Add these if needed based on frontend requirements.
    }
} 