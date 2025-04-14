using System.Collections.Generic;

namespace TravelSite.Models.DTOs
{
    public class FlightOption
    {
        public List<FlightSegment>? Flights { get; set; }
        public List<Layover>? Layovers { get; set; }
        public int TotalDuration { get; set; } // In minutes
        public decimal Price { get; set; } // Assuming decimal for currency
        public required string Type { get; set; } // e.g., "Round trip"
        public CarbonEmissions? CarbonEmissions { get; set; }
        public string? DepartureToken { get; set; }
        public string? BookingToken { get; set; }
        // Note: The API doc also shows airline_logo and extensions at this level,
        // decide if they are needed based on frontend requirements.
    }
} 