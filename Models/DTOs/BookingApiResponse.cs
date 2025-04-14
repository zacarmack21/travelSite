using System.Collections.Generic;
using Newtonsoft.Json;
// Add import for System.Text.Json
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
        // [JsonProperty("search_metadata")]
        // [JsonPropertyName("search_metadata")]
        // public SearchMetadata? SearchMetadata { get; set; }
        // [JsonProperty("search_parameters")]
        // [JsonPropertyName("search_parameters")]
        // public SearchParameters? SearchParameters { get; set; }

        [JsonProperty("selected_flights")]
        [JsonPropertyName("selected_flights")]
        public List<FlightOption>? SelectedFlights { get; set; } // Reusing FlightOption DTO

        [JsonProperty("baggage_prices")]
        [JsonPropertyName("baggage_prices")]
        public BaggagePrices? BaggagePrices { get; set; }

        [JsonProperty("booking_options")]
        [JsonPropertyName("booking_options")]
        public List<BookingOptionDetail>? BookingOptions { get; set; }

        [JsonProperty("price_insights")]
        [JsonPropertyName("price_insights")]
        public PriceInsights? PriceInsights { get; set; } // Reusing PriceInsights DTO

        // Use attribute if ErrorMessage needs to be serialized with this name
        [JsonProperty("error_message")]
        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }
    }
} 