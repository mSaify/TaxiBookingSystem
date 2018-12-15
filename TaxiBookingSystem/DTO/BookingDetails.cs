using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiBookingSystem.DTO
{
    public class BookingDetails
    {
        public int TaxiId { get; set; }

        public int CustomerId { get; set; }

        public int TotalTimeForJourney { get; set; }
    }
}
