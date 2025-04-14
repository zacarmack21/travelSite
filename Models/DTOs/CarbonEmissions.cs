namespace TravelSite.Models.DTOs
{
    public class CarbonEmissions
    {
        public int? ThisFlight { get; set; } // In grams
        public int? TypicalForThisRoute { get; set; } // In grams
        public int? DifferencePercent { get; set; }
    }
} 