using System.Threading.Tasks;
using TravelSite.Models.DTOs;

namespace TravelSite.Services
{
    public interface IFlightSearchService
    {
        Task<FlightSearchResponse> SearchFlightsAsync(FlightSearchRequest request);
    }
} 