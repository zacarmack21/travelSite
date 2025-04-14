using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class FlightSearchResponse
    {
        [JsonPropertyName("best_flights")]
        public List<FlightOption>? BestFlights { get; set; }

        [JsonPropertyName("other_flights")]
        public List<FlightOption>? OtherFlights { get; set; }

        [JsonPropertyName("price_insights")]
        public PriceInsights? PriceInsights { get; set; }

        // Removed SelectedFlights and BookingOptions as they don't exist in the root of the actual API response
        // The return flight options are contained within BestFlights/OtherFlights
        // Booking tokens are within the FlightOption class

        [JsonPropertyName("baggage_prices")]
        public BaggagePrices? BaggagePrices { get; set; }

        [JsonIgnore]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("search_metadata")]
        public SearchMetadata? SearchMetadata { get; set; }

        [JsonPropertyName("search_parameters")]
        public SearchParameters? SearchParameters { get; set; }

        [JsonPropertyName("error")]
        public string? ApiError { get; set; }
    }

    // Removing SelectedFlight class as it's not used and inherits from FlightOption anyway
    /*
    public class SelectedFlight : FlightOption
    {
        // SelectedFlight might have slightly different structure or additional properties
        // compared to FlightOption from the initial search. Review bookingResponse.txt.
        // For now, assume it's similar enough to FlightOption for display purposes.
        // If specific fields are missing or different, adjust this class and FlightOption.
    }
    */

    // Removing BookingOption and BookingProviderDetails as they don't map to the actual API response structure
    /*
    public class BookingOption
    {
        [JsonPropertyName("together")]
        public BookingProviderDetails? Together { get; set; }
        // Add other types if they exist (e.g., "split")
    }

    public class BookingProviderDetails
    {
        [JsonPropertyName("book_with")]
        public string? BookWith { get; set; }

        [JsonPropertyName("airline_logos")]
        public List<string>? AirlineLogos { get; set; }

        [JsonPropertyName("marketed_as")]
        public List<string>? MarketedAs { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("local_prices")]
        public List<LocalPrice>? LocalPrices { get; set; }

        [JsonPropertyName("option_title")]
        public string? OptionTitle { get; set; }

        [JsonPropertyName("extensions")]
        public List<string>? Extensions { get; set; }

        [JsonPropertyName("baggage_prices")]
        public List<string>? BaggagePrices { get; set; }

        [JsonPropertyName("booking_request")]
        public BookingRequest? BookingRequest { get; set; }
    }
    */

    public class BaggagePrices
    {
        [JsonPropertyName("together")]
        public List<string>? Together { get; set; }
    }

    public class LocalPrice
    {
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }

    public class BookingRequest
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("post_data")]
        public string? PostData { get; set; }
    }

    // Add classes for SearchMetadata and SearchParameters if needed
    public class SearchMetadata { /* ... properties based on JSON ... */ }
    public class SearchParameters { /* ... properties based on JSON ... */ }
} 