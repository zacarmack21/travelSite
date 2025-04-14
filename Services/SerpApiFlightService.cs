using System;
using System.Net.Http;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TravelSite.Models.DTOs;

namespace TravelSite.Services
{
    public class SerpApiFlightService : IFlightSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SerpApiFlightService> _logger;
        private readonly string _serpApiBaseUrl = "https://serpapi.com/search.json"; // Base URL for SerpApi

        public SerpApiFlightService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<SerpApiFlightService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public FlightSearchResponse SearchFlights(FlightSearchRequest request)
        {
            var apiKey = _configuration["SerpApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("SerpApi API Key is not configured.");
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
            if (!string.IsNullOrEmpty(request.DepartureToken))
            {
                query["departure_token"] = request.DepartureToken;
            }
            // TODO: Add other optional parameters from FlightSearchRequest as needed (e.g., travel_class, stops, etc.)

            string requestUrl = $"{_serpApiBaseUrl}?{query}";
            _logger.LogInformation("Requesting SerpApi URL: {RequestUrl}", requestUrl);

            try
            {
                var response = client.GetAsync(requestUrl).GetAwaiter().GetResult();
                var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                _logger.LogInformation("SerpApi Raw Response Status: {StatusCode}", response.StatusCode);
                _logger.LogDebug("SerpApi Raw JSON Response: {JsonResponse}", jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        // PropertyNameCaseInsensitive = true
                    };
                    FlightSearchResponse? flightResponse = null;
                    try
                    {
                        flightResponse = JsonSerializer.Deserialize<FlightSearchResponse>(jsonResponse, options);
                        _logger.LogDebug("Deserialized BestFlights count: {Count}", flightResponse?.BestFlights?.Count ?? 0);
                        _logger.LogDebug("Deserialized OtherFlights count: {Count}", flightResponse?.OtherFlights?.Count ?? 0);
                        _logger.LogDebug("Deserialized PriceInsights lowest price: {Price}", flightResponse?.PriceInsights?.LowestPrice);

                        if (flightResponse == null)
                        {
                            _logger.LogWarning("Deserialization resulted in a null FlightSearchResponse object.");
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON Deserialization failed for SerpApi response. Raw JSON: {JsonResponse}", jsonResponse);
                        return new FlightSearchResponse { ErrorMessage = $"Error processing flight data structure: {jsonEx.Message}" };
                    }

                    var jsonDoc = JsonDocument.Parse(jsonResponse);
                    if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        string? apiError = errorElement.GetString();
                        _logger.LogWarning("SerpApi response contained an error message: {ApiError}", apiError);
                        return new FlightSearchResponse { ErrorMessage = apiError ?? "SerpApi returned an unspecified error." };
                    }

                    return flightResponse ?? new FlightSearchResponse { ErrorMessage = "Failed to deserialize SerpApi response (Result was null)." };
                }
                else
                {
                    _logger.LogError("SerpApi request failed with status code: {StatusCode}. Response Body: {ErrorContent}", response.StatusCode, jsonResponse);
                    return new FlightSearchResponse { ErrorMessage = $"Error retrieving flight data (Status: {response.StatusCode})" };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error contacting SerpApi.");
                return new FlightSearchResponse { ErrorMessage = $"Network error contacting flight service: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing SerpApi JSON response after successful HTTP call.");
                return new FlightSearchResponse { ErrorMessage = $"Error parsing flight data: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in flight search service.");
                return new FlightSearchResponse { ErrorMessage = $"An unexpected server error occurred: {ex.Message}" };
            }
        }
    }
} 