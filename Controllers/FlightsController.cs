using Microsoft.AspNetCore.Mvc;
using TravelSite.Models.DTOs;
using TravelSite.Services;
using TravelSite.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TravelSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightSearchService _flightSearchService;
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(IFlightSearchService flightSearchService, ILogger<FlightsController> logger)
        {
            _flightSearchService = flightSearchService;
            _logger = logger;
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(FlightSearchResponse), 200)] // OK
        [ProducesResponseType(typeof(string), 400)] // Bad Request
        [ProducesResponseType(typeof(string), 500)] // Internal Server Error
        public async Task<IActionResult> SearchFlightsAsync([FromBody] FlightSearchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Received flight search request for {DepartureId} to {ArrivalId} on {OutboundDate}",
                request.DepartureId, request.ArrivalId, request.OutboundDate);

            var response = await _flightSearchService.SearchFlightsAsync(request);

            if (response == null)
            {
                _logger.LogError("Flight search service returned null unexpectedly.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred during flight search.");
            }

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                _logger.LogWarning("Flight search failed: {ErrorMessage}", response.ErrorMessage);
                if (response.ErrorMessage.Contains("API Key is not configured"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server configuration error.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving flight data: {response.ErrorMessage}");
            }

            _logger.LogInformation("Flight search successful, returning {BestFlightsCount} best flights and {OtherFlightsCount} other flights.",
                response.BestFlights?.Count ?? 0, response.OtherFlights?.Count ?? 0);

            return Ok(response);
        }

        [HttpPost("booking-options")]
        [ProducesResponseType(typeof(BookingApiResponse), 200)] // OK
        [ProducesResponseType(typeof(string), 400)] // Bad Request
        [ProducesResponseType(typeof(string), 500)] // Internal Server Error
        public IActionResult GetBookingOptions([FromBody] BookingOptionsRequest request)
        {
            if (!ModelState.IsValid || request.OriginalSearchRequest == null || string.IsNullOrEmpty(request.BookingToken))
            {
                _logger.LogWarning("Invalid booking options request received.");
                return BadRequest("Invalid request data. Booking token and original search request are required.");
            }

            _logger.LogInformation("Received booking options request with token: {BookingToken}", request.BookingToken);

            var response = _flightSearchService.GetBookingOptions(request.BookingToken, request.OriginalSearchRequest);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                _logger.LogWarning("Fetching booking options failed: {ErrorMessage}", response.ErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving booking options: {response.ErrorMessage}");
            }

            _logger.LogInformation("Booking options retrieved successfully, returning {OptionsCount} options.", response.BookingOptions?.Count ?? 0);
            return Ok(response);
        }
    }
} 