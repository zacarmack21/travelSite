using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TravelSite.Models.DTOs;
using System.Threading.Tasks;
using TravelSite.Models; // Added for FlightSearchRequest in new method

namespace TravelSite.Services
{
    public class SerpApiFlightService : IFlightSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SerpApiFlightService> _logger;
        private readonly string _serpApiBaseUrl = "https://serpapi.com/search.json"; // Base URL for SerpApi
        private readonly string _apiKey; // Store API key after reading it once

        public SerpApiFlightService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<SerpApiFlightService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            // Get API Key in constructor to avoid repeated lookups and null checks
            _apiKey = _configuration["SerpApiKey"] ?? throw new InvalidOperationException("SerpApi API Key is not configured.");
        }

        public async Task<FlightSearchResponse?> SearchFlightsAsync(FlightSearchRequest request)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                // This check is now redundant due to constructor check, but kept for safety
                _logger.LogError("SerpApi API Key is not configured.");
                return new FlightSearchResponse { ErrorMessage = "SerpApi API Key is not configured." };
            }

            var client = _httpClientFactory.CreateClient("SerpApiClient");

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["engine"] = "google_flights";
            query["api_key"] = _apiKey;
            query["departure_id"] = request.DepartureId;
            query["arrival_id"] = request.ArrivalId;
            query["outbound_date"] = request.OutboundDate;
            query["type"] = request.Type.ToString();
            query["adults"] = request.Adults.ToString();

            // Add optional parameters
            if (!string.IsNullOrEmpty(request.ReturnDate) && request.Type == 1) query["return_date"] = request.ReturnDate;
            if (request.Children.HasValue && request.Children > 0) query["children"] = request.Children.Value.ToString();
            if (request.InfantsInSeat.HasValue && request.InfantsInSeat > 0) query["infants_in_seat"] = request.InfantsInSeat.Value.ToString();
            if (request.InfantsOnLap.HasValue && request.InfantsOnLap > 0) query["infants_on_lap"] = request.InfantsOnLap.Value.ToString();
            if (!string.IsNullOrEmpty(request.Hl)) query["hl"] = request.Hl; else query["hl"] = "en"; // Default hl if not provided
            if (!string.IsNullOrEmpty(request.Gl)) query["gl"] = request.Gl; else query["gl"] = "us"; // Default gl if not provided
            if (!string.IsNullOrEmpty(request.Currency)) query["currency"] = request.Currency; else query["currency"] = "USD"; // Default currency if not provided
            if (!string.IsNullOrEmpty(request.DepartureToken)) query["departure_token"] = request.DepartureToken;
            // Add other optional parameters from FlightSearchRequest if needed

            string requestUrl = $"{_serpApiBaseUrl}?{query}";
            _logger.LogInformation("Requesting SerpApi URL for Search: {RequestUrl}", requestUrl);

            try
            {
                var response = await client.GetAsync(requestUrl);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SerpApi Search Response Status: {StatusCode}", response.StatusCode);
                _logger.LogDebug("SerpApi Search Raw JSON Response: {JsonResponse}", jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    // Check for explicit error in JSON before deserializing
                    using var jsonDoc = JsonDocument.Parse(jsonResponse);
                    if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        string? apiError = errorElement.GetString();
                        _logger.LogWarning("SerpApi search response contained an error message: {ApiError}", apiError);
                        return new FlightSearchResponse { ErrorMessage = apiError ?? "SerpApi returned an unspecified error." };
                    }

                    try
                    {
                        // Use case-insensitive deserialization for flexibility
                        var options = new JsonSerializerOptions 
                        { 
                            PropertyNameCaseInsensitive = true,
                            // Allow reading numbers from strings and writing numbers as strings if needed
                            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString 
                        };
                        var flightResponse = JsonSerializer.Deserialize<FlightSearchResponse>(jsonResponse, options);
                        if (flightResponse == null)
                        {
                            _logger.LogWarning("Deserialization resulted in a null FlightSearchResponse object.");
                            return new FlightSearchResponse { ErrorMessage = "Failed to deserialize SerpApi response (Result was null)." };
                        }
                        return flightResponse;
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON Deserialization failed for SerpApi search response. Raw JSON: {JsonResponse}", jsonResponse);
                        return new FlightSearchResponse { ErrorMessage = $"Error processing flight data structure: {jsonEx.Message}" };
                    }
                }
                else
                {
                    _logger.LogError("SerpApi search request failed with status code: {StatusCode}. Response Body: {ErrorContent}", response.StatusCode, jsonResponse);
                    // Try to parse error from body if possible
                    string errorMessage = $"Error retrieving flight data (Status: {response.StatusCode})";
                    try
                    {
                        using var jsonDoc = JsonDocument.Parse(jsonResponse);
                        if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                        {
                           errorMessage = errorElement.GetString() ?? errorMessage;
                        }
                    } catch {} // Ignore parsing errors, use default message
                    return new FlightSearchResponse { ErrorMessage = errorMessage };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error contacting SerpApi during search.");
                return new FlightSearchResponse { ErrorMessage = $"Network error contacting flight service: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                 // This catch might be less likely now with the check inside success block
                _logger.LogError(ex, "Error parsing SerpApi JSON search response.");
                return new FlightSearchResponse { ErrorMessage = $"Error parsing flight data: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in flight search service.");
                return new FlightSearchResponse { ErrorMessage = $"An unexpected server error occurred: {ex.Message}" };
            }
        }

        // Implementation for GetBookingOptionsAsync
        public async Task<BookingApiResponse?> GetBookingOptionsAsync(string bookingToken, FlightSearchRequest originalRequest)
        {
             if (string.IsNullOrEmpty(_apiKey))
            {
                 _logger.LogError("SerpApi API Key is not configured.");
                return new BookingApiResponse { /* Add ErrorMessage property if desired */ }; // Or throw exception
            }
             if (string.IsNullOrEmpty(bookingToken))
            {
                _logger.LogWarning("Booking token was null or empty.");
                return new BookingApiResponse { /* ErrorMessage */ };
            }

            var client = _httpClientFactory.CreateClient("SerpApiClient");

            // Construct the query parameters for booking options
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["engine"] = "google_flights";
            query["api_key"] = _apiKey;
            query["booking_token"] = bookingToken;

            // Add necessary parameters from original request (API requires some even with token)
            query["departure_id"] = originalRequest.DepartureId;
            query["arrival_id"] = originalRequest.ArrivalId;
            query["outbound_date"] = originalRequest.OutboundDate; // May be ignored by API
            query["type"] = originalRequest.Type.ToString(); // May be ignored by API
            query["adults"] = originalRequest.Adults.ToString(); // May be ignored by API

            // Add return_date if it's a round trip (Type == 1)
            if (originalRequest.Type == 1 && !string.IsNullOrEmpty(originalRequest.ReturnDate))
            {
                query["return_date"] = originalRequest.ReturnDate;
            }

            // Add optional but potentially required localization/currency params
            query["hl"] = !string.IsNullOrEmpty(originalRequest.Hl) ? originalRequest.Hl : "en";
            query["gl"] = !string.IsNullOrEmpty(originalRequest.Gl) ? originalRequest.Gl : "us";
            query["currency"] = !string.IsNullOrEmpty(originalRequest.Currency) ? originalRequest.Currency : "USD";

            // Parameters like return_date, filters (stops, airlines etc.) are likely ignored
            // by the API when booking_token is present, so we omit them.

            string requestUrl = $"{_serpApiBaseUrl}?{query}";
            _logger.LogInformation("Requesting SerpApi URL for Booking Options: {RequestUrl}", requestUrl);

            try
            {
                var response = await client.GetAsync(requestUrl);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SerpApi Booking Options Response Status: {StatusCode}", response.StatusCode);
                _logger.LogDebug("SerpApi Booking Options Raw JSON Response: {JsonResponse}", jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    // Check for explicit error in JSON
                    using var jsonDoc = JsonDocument.Parse(jsonResponse);
                     if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        string? apiError = errorElement.GetString();
                        _logger.LogWarning("SerpApi booking options response contained an error message: {ApiError}", apiError);
                        return new BookingApiResponse { /* ErrorMessage */ };
                    }

                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var bookingResponse = JsonSerializer.Deserialize<BookingApiResponse>(jsonResponse, options);
                         if (bookingResponse == null)
                        {
                            _logger.LogWarning("Deserialization resulted in a null BookingApiResponse object.");
                            return new BookingApiResponse { /* ErrorMessage */ };
                        }
                        return bookingResponse;
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON Deserialization failed for SerpApi booking options response. Raw JSON: {JsonResponse}", jsonResponse);
                        return new BookingApiResponse { /* ErrorMessage */ };
                    }
                }
                else
                {
                    _logger.LogError("SerpApi booking options request failed with status code: {StatusCode}. Response Body: {ErrorContent}", response.StatusCode, jsonResponse);
                    // Try parse error
                     string errorMessage = $"Error retrieving booking data (Status: {response.StatusCode})";
                    try
                    {
                        using var jsonDoc = JsonDocument.Parse(jsonResponse);
                        if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                        {
                           errorMessage = errorElement.GetString() ?? errorMessage;
                        }
                    } catch {}
                    return new BookingApiResponse { /* ErrorMessage = errorMessage */ };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error contacting SerpApi during booking options request.");
                return new BookingApiResponse { /* ErrorMessage */ };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing SerpApi JSON booking options response.");
                 return new BookingApiResponse { /* ErrorMessage */ };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in booking options service.");
                return new BookingApiResponse { /* ErrorMessage */ };
            }
        }

        // Reverted to synchronous implementation
        public BookingApiResponse GetBookingOptions(string bookingToken, FlightSearchRequest originalRequest)
        {
             if (string.IsNullOrEmpty(_apiKey))
            {
                 _logger.LogError("SerpApi API Key is not configured.");
                return new BookingApiResponse { ErrorMessage = "SerpApi API Key is not configured." };
            }
             if (string.IsNullOrEmpty(bookingToken))
            {
                _logger.LogWarning("Booking token was null or empty.");
                return new BookingApiResponse { ErrorMessage = "Booking token cannot be empty." };
            }

            var client = _httpClientFactory.CreateClient("SerpApiClient");

            // Construct the query parameters for booking options
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["engine"] = "google_flights";
            query["api_key"] = _apiKey;
            query["booking_token"] = bookingToken;

            // Add necessary parameters from original request (API requires some even with token)
            query["departure_id"] = originalRequest.DepartureId;
            query["arrival_id"] = originalRequest.ArrivalId;
            query["outbound_date"] = originalRequest.OutboundDate; // May be ignored by API
            query["type"] = originalRequest.Type.ToString(); // May be ignored by API
            query["adults"] = originalRequest.Adults.ToString(); // May be ignored by API

            // Add return_date if it's a round trip (Type == 1)
            if (originalRequest.Type == 1 && !string.IsNullOrEmpty(originalRequest.ReturnDate))
            {
                query["return_date"] = originalRequest.ReturnDate;
            }

            // Add optional but potentially required localization/currency params
            query["hl"] = !string.IsNullOrEmpty(originalRequest.Hl) ? originalRequest.Hl : "en";
            query["gl"] = !string.IsNullOrEmpty(originalRequest.Gl) ? originalRequest.Gl : "us";
            query["currency"] = !string.IsNullOrEmpty(originalRequest.Currency) ? originalRequest.Currency : "USD";

            string requestUrl = $"{_serpApiBaseUrl}?{query}";
            _logger.LogInformation("Requesting SerpApi URL for Booking Options: {RequestUrl}", requestUrl);

            try
            {
                // WARNING: Blocking on async calls can lead to deadlocks.
                var response = client.GetAsync(requestUrl).GetAwaiter().GetResult();
                var jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                _logger.LogInformation("SerpApi Booking Options Response Status: {StatusCode}", response.StatusCode);
                _logger.LogDebug("SerpApi Booking Options Raw JSON Response: {JsonResponse}", jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    using var jsonDoc = JsonDocument.Parse(jsonResponse);
                     if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                    {
                        string? apiError = errorElement.GetString();
                        _logger.LogWarning("SerpApi booking options response contained an error message: {ApiError}", apiError);
                        return new BookingApiResponse { ErrorMessage = apiError ?? "SerpApi returned an unspecified error." };
                    }

                    try
                    {
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var bookingResponse = JsonSerializer.Deserialize<BookingApiResponse>(jsonResponse, options);
                         if (bookingResponse == null)
                        {
                            _logger.LogWarning("Deserialization resulted in a null BookingApiResponse object.");
                            return new BookingApiResponse { ErrorMessage = "Failed to deserialize booking options response." };
                        }
                        return bookingResponse;
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "JSON Deserialization failed for SerpApi booking options response. Raw JSON: {JsonResponse}", jsonResponse);
                        return new BookingApiResponse { ErrorMessage = $"Error processing booking options data structure: {jsonEx.Message}" };
                    }
                }
                else
                {
                    _logger.LogError("SerpApi booking options request failed with status code: {StatusCode}. Response Body: {ErrorContent}", response.StatusCode, jsonResponse);
                    string errorMessage = $"Error retrieving booking data (Status: {response.StatusCode})";
                    try
                    {
                        using var jsonDoc = JsonDocument.Parse(jsonResponse);
                        if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
                        {
                           errorMessage = errorElement.GetString() ?? errorMessage;
                        }
                    } catch {}
                    return new BookingApiResponse { ErrorMessage = errorMessage };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error contacting SerpApi during booking options request.");
                return new BookingApiResponse { ErrorMessage = $"Network error contacting booking service: {ex.Message}" };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing SerpApi JSON booking options response.");
                 return new BookingApiResponse { ErrorMessage = $"Error parsing booking data: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in booking options service.");
                return new BookingApiResponse { ErrorMessage = $"An unexpected server error occurred: {ex.Message}" };
            }
        }

        // Keeping the synchronous version for now, but marking as obsolete or removing later might be good
        // Consider refactoring the controller to use the async version.
        public FlightSearchResponse SearchFlights(FlightSearchRequest request)
        {
           // Simple wrapper to call the async version and wait
           // WARNING: Blocking on async code like this (.GetAwaiter().GetResult()) can lead to deadlocks
           // in some contexts (like ASP.NET classic). It's generally better to make the entire call stack async.
           // This is provided for minimal change from the original code structure.
           _logger.LogWarning("Calling synchronous SearchFlights wrapper. Consider switching to async.");
           try
           {
               var result = SearchFlightsAsync(request).GetAwaiter().GetResult();
               return result ?? new FlightSearchResponse { ErrorMessage = "Async search returned null." };
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error in synchronous SearchFlights wrapper.");
                // Ensure an ErrorMessage is returned
               return new FlightSearchResponse { ErrorMessage = $"An error occurred: {ex.Message}" };
           }
        }
    }
} 