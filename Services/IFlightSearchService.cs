using System.Threading.Tasks;
using TravelSite.Models;
using TravelSite.Models.DTOs;

namespace TravelSite.Services
{
    public interface IFlightSearchService
    {
        FlightSearchResponse SearchFlights(FlightSearchRequest request);

        Task<FlightSearchResponse?> SearchFlightsAsync(FlightSearchRequest request);

        BookingApiResponse GetBookingOptions(string bookingToken, FlightSearchRequest originalRequest);
    }
} 