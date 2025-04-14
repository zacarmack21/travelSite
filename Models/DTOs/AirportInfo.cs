namespace TravelSite.Models.DTOs
{
    public class AirportInfo
    {
        public required string Name { get; set; }
        public required string Id { get; set; } // Airport code (e.g., "AUS")
        public required string Time { get; set; } // Departure/Arrival time (e.g., "2023-10-03 15:10")
    }
} 