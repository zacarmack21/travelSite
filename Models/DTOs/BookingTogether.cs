using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingTogether
    {
        [JsonProperty("book_with")]
        [JsonPropertyName("book_with")]
        public string? BookWith { get; set; } // e.g., "JAL", "Kiwi.com"

        [JsonProperty("airline_logos")]
        [JsonPropertyName("airline_logos")]
        public List<string>? AirlineLogos { get; set; } // URLs

        [JsonProperty("marketed_as")]
        [JsonPropertyName("marketed_as")]
        public List<string>? MarketedAs { get; set; } // Flight numbers

        [JsonProperty("price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonProperty("option_title")]
        [JsonPropertyName("option_title")]
        public string? OptionTitle { get; set; } // e.g., "Wanna Get Away"

        [JsonProperty("extensions")]
        [JsonPropertyName("extensions")]
        public List<string>? Extensions { get; set; } // e.g., ["Priority boarding for a fee", ...]

        [JsonProperty("local_prices")]
        [JsonPropertyName("local_prices")]
        public List<LocalPrice>? LocalPrices { get; set; }

        [JsonProperty("baggage_prices")]
        [JsonPropertyName("baggage_prices")]
        public List<string>? BaggagePrices { get; set; } // Note: Duplicates BaggagePrices structure slightly

        [JsonProperty("booking_request")]
        [JsonPropertyName("booking_request")]
        public BookingRequest? BookingRequest { get; set; }
    }
} 