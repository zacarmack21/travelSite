namespace TravelSite.Models.DTOs
{
    public class FlightSegment
    {
        public required AirportInfo DepartureAirport { get; set; }
        public required AirportInfo ArrivalAirport { get; set; }
        public int Duration { get; set; } // In minutes
        public required string Airline { get; set; }
        public required string AirlineLogo { get; set; } // URL
        public required string FlightNumber { get; set; }
        public required string TravelClass { get; set; }
        public string? Legroom { get; set; } // e.g., "31 in"
        // Note: The API doc also shows airplane, extensions, ticket_also_sold_by, overnight, etc.
        // Add these if needed based on frontend requirements.
    }
} 