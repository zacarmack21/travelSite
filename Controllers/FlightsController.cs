using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TravelSite.Models.DTOs;
using TravelSite.Services;

namespace TravelSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightSearchService _flightSearchService;
        // Optional: Add ILogger if needed

        public FlightsController(IFlightSearchService flightSearchService)
        {
            _flightSearchService = flightSearchService;
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

            // Optional: Add more specific validation here if needed
            // e.g., check date formats, departure/arrival codes, etc.

            var response = await _flightSearchService.SearchFlightsAsync(request);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                // Log the error message if logging is configured
                // Determine if it's a client error (400) or server error (500)
                // For simplicity, returning 500 for any service-level error for now.
                // A more robust implementation might inspect the error type.
                if (response.ErrorMessage.Contains("API Key is not configured"))
                {
                    // Log critical configuration error
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server configuration error.");
                }
                if (response.ErrorMessage.Contains("failed with status code"))
                {
                    // Treat API errors as internal server issues for the client
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving flight data.");
                }
                 if (response.ErrorMessage.Contains("Network error") || response.ErrorMessage.Contains("deserialize"))
                {
                   return StatusCode(StatusCodes.Status500InternalServerError, "Error processing flight data.");
                }
                // Consider other specific errors or default to a generic server error
                return StatusCode(StatusCodes.Status500InternalServerError, response.ErrorMessage); 
            }

            return Ok(response);
        }
    }
} 