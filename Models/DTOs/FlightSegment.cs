using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSegment
    {
        [JsonPropertyName("departure_airport")]
        public required AirportInfo DepartureAirport { get; set; }

        [JsonPropertyName("arrival_airport")]
        public required AirportInfo ArrivalAirport { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; } // In minutes

        [JsonPropertyName("airline")]
        public required string Airline { get; set; }

        [JsonPropertyName("airline_logo")]
        public required string AirlineLogo { get; set; } // URL

        [JsonPropertyName("flight_number")]
        public required string FlightNumber { get; set; }

        [JsonPropertyName("travel_class")]
        public required string TravelClass { get; set; }

        [JsonPropertyName("legroom")]
        public string? Legroom { get; set; } // e.g., "31 in"
        // Note: The API doc also shows airplane, extensions, ticket_also_sold_by, overnight, etc.
        // Add these if needed based on frontend requirements.
    }
} 