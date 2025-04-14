using System.Collections.Generic;
using System.Text.Json.Serialization;
// Assuming FlightOption and PriceInsights are in the same namespace or using directives are added
// namespace TravelSite.Models.DTOs; // Uncomment if FlightOption/PriceInsights are here

namespace TravelSite.Models.DTOs
{
    public class BookingApiResponse
    {
        // We might not need SearchMetadata and SearchParameters if the frontend doesn't use them
        // But including them for completeness based on the sample response.
        // Consider creating generic SearchMetadata and SearchParameters DTOs if reused elsewhere.

        // Example: Add properties for SearchMetadata and SearchParameters if needed
        // [JsonPropertyName("search_metadata")]
        // public SearchMetadata? SearchMetadata { get; set; }
        // [JsonPropertyName("search_parameters")]
        // public SearchParameters? SearchParameters { get; set; }

        [JsonPropertyName("selected_flights")]
        public List<FlightOption>? SelectedFlights { get; set; } // Reusing FlightOption DTO

        [JsonPropertyName("baggage_prices")]
        public BaggagePrices? BaggagePrices { get; set; }

        [JsonPropertyName("booking_options")]
        public List<BookingOptionDetail>? BookingOptions { get; set; }

        [JsonPropertyName("price_insights")]
        public PriceInsights? PriceInsights { get; set; } // Reusing PriceInsights DTO

        // Add property to hold potential error messages from the service
        [JsonIgnore] // Don't expect this in the API response JSON itself
        public string? ErrorMessage { get; set; }
    }
} 