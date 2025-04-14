using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSearchRequest
    {
        public required string DepartureId { get; set; }
        public required string ArrivalId { get; set; }
        public required string OutboundDate { get; set; } // Format: YYYY-MM-DD
        public string? ReturnDate { get; set; } // Format: YYYY-MM-DD, Nullable for one-way
        public int Type { get; set; } // 1=Round trip, 2=One way
        public int Adults { get; set; } = 1; // Default to 1 adult
        public int? Children { get; set; }
        public int? InfantsInSeat { get; set; }
        public int? InfantsOnLap { get; set; }
        public string? Hl { get; set; } // Language
        public string? Gl { get; set; } // Country
        public string? Currency { get; set; } // e.g., USD
        [JsonPropertyName("departure_token")]
        public string? DepartureToken { get; set; } // Token for fetching return flights
    }
} 