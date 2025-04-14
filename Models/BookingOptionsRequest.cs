using System.ComponentModel.DataAnnotations;
using TravelSite.Models.DTOs;

namespace TravelSite.Models
{
    public class BookingOptionsRequest
    {
        [Required]
        public string? BookingToken { get; set; }

        [Required]
        public FlightSearchRequest? OriginalSearchRequest { get; set; }
    }
} 