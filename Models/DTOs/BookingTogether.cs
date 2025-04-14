using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelSite.Models.DTOs
{
    public class BookingTogether
    {
        [JsonPropertyName("book_with")]
        public string? BookWith { get; set; } // e.g., "JAL", "Kiwi.com"

        [JsonPropertyName("airline_logos")]
        public List<string>? AirlineLogos { get; set; } // URLs

        [JsonPropertyName("marketed_as")]
        public List<string>? MarketedAs { get; set; } // Flight numbers

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("local_prices")]
        public List<LocalPrice>? LocalPrices { get; set; }

        [JsonPropertyName("baggage_prices")]
        public List<string>? BaggagePrices { get; set; } // Note: Duplicates BaggagePrices structure slightly

        [JsonPropertyName("booking_request")]
        public BookingRequest? BookingRequest { get; set; }
    }
} 