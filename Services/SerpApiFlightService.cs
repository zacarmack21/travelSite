using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using TravelSite.Models.DTOs;

namespace TravelSite.Services
{
    public class SerpApiFlightService : IFlightSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _serpApiBaseUrl = "https://serpapi.com/search.json"; // Base URL for SerpApi

        public SerpApiFlightService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<FlightSearchResponse> SearchFlightsAsync(FlightSearchRequest request)
        {
            var apiKey = _configuration["SerpApi:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                // Consider logging this error
                return new FlightSearchResponse { ErrorMessage = "SerpApi API Key is not configured." };
            }

            var client = _httpClientFactory.CreateClient("SerpApiClient"); // Use a named client if needed

            // Construct the query parameters
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["engine"] = "google_flights";
            query["api_key"] = apiKey;
            query["departure_id"] = request.DepartureId;
            query["arrival_id"] = request.ArrivalId;
            query["outbound_date"] = request.OutboundDate;
            query["type"] = request.Type.ToString();
            query["adults"] = request.Adults.ToString();

            // Add optional parameters if they have values
            if (!string.IsNullOrEmpty(request.ReturnDate) && request.Type == 1) // Return date only for round trip
            {
                query["return_date"] = request.ReturnDate;
            }
            if (request.Children.HasValue && request.Children > 0)
            {
                query["children"] = request.Children.Value.ToString();
            }
            if (request.InfantsInSeat.HasValue && request.InfantsInSeat > 0)
            {
                query["infants_in_seat"] = request.InfantsInSeat.Value.ToString();
            }
            if (request.InfantsOnLap.HasValue && request.InfantsOnLap > 0)
            {
                query["infants_on_lap"] = request.InfantsOnLap.Value.ToString();
            }
            if (!string.IsNullOrEmpty(request.Hl))
            {
                query["hl"] = request.Hl;
            }
            if (!string.IsNullOrEmpty(request.Gl))
            {
                query["gl"] = request.Gl;
            }
            if (!string.IsNullOrEmpty(request.Currency))
            {
                query["currency"] = request.Currency;
            }
            // TODO: Add other optional parameters from FlightSearchRequest as needed (e.g., travel_class, stops, etc.)

            string requestUrl = $"{_serpApiBaseUrl}?{query}";

            try
            {
                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize using System.Text.Json, ensure property names match API response
                    // (or use [JsonPropertyName] attributes in DTOs)
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Handle potential case differences (e.g., best_flights vs BestFlights)
                    };
                    var flightResponse = JsonSerializer.Deserialize<FlightSearchResponse>(jsonResponse, options);

                    // Basic check if SerpApi itself indicated an error in the response structure (if applicable)
                    // The specific structure depends on SerpApi's error reporting in JSON.
                    // For now, we assume deserialization covers the main success/error paths.
                    // if (flightResponse != null && !string.IsNullOrEmpty(flightResponse.SomeErrorFieldFromApi)) {
                    //     return new FlightSearchResponse { ErrorMessage = flightResponse.SomeErrorFieldFromApi };
                    // }

                    return flightResponse ?? new FlightSearchResponse { ErrorMessage = "Failed to deserialize SerpApi response." };
                }
                else
                {
                    // Log the error status code and potentially the response body
                    string errorContent = await response.Content.ReadAsStringAsync();
                    // Log errorContent
                    return new FlightSearchResponse { ErrorMessage = $"SerpApi request failed with status code: {response.StatusCode}" };
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                return new FlightSearchResponse { ErrorMessage = $"Network error contacting SerpApi: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                // Log the exception
                return new FlightSearchResponse { ErrorMessage = $"Error deserializing SerpApi response: {ex.Message}" };
            }
            catch (Exception ex) // Catch unexpected errors
            {
                // Log the exception
                return new FlightSearchResponse { ErrorMessage = $"An unexpected error occurred: {ex.Message}" };
            }
        }
    }
} 