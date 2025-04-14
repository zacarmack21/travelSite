using System.Threading.Tasks;
using TravelSite.Models.DTOs;

namespace TravelSite.Services
{
    public interface IFlightSearchService
    {
        FlightSearchResponse SearchFlights(FlightSearchRequest request);
    }
} 