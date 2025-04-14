using System.Collections.Generic;

namespace TravelSite.Models.DTOs
{
    public class FlightSearchResponse
    {
        public List<FlightOption>? BestFlights { get; set; }
        public List<FlightOption>? OtherFlights { get; set; }
        public PriceInsights? PriceInsights { get; set; }
        public string? ErrorMessage { get; set; }
    }
} 