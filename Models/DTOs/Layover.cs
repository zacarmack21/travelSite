namespace TravelSite.Models.DTOs
{
    public class Layover
    {
        public int Duration { get; set; } // In minutes
        public required string Name { get; set; } // Airport name
        public required string Id { get; set; } // Airport code
        // Note: API doc also includes 'overnight' boolean, add if needed.
    }
} 