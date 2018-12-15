using Microsoft.AspNetCore.Mvc;
using TaxiBookingSystemCore;
using TaxiBookingSystem.DTO;
using TaxiBookingSystemEntities;

namespace TaxiBookingSystem.Controllers
{   
    public class TaxiBookingController : Controller
    {
        private TaxiBookingService _taxiBookingService { get; } = new TaxiBookingService();

        public TaxiBookingController()
        {
        }
        
        [HttpGet]
        [Route("api/taxi/{taxiId}")]
        public TaxiDetailDto Get(int taxiId)
        {
            var taxi = TaxiFleet.Instance.GetTaxi(taxiId);
            return new TaxiDetailDto(taxi).ConvertToDto();
            

        }
        
        [HttpPost]
        [Route("api/book")]
        
        public BookingDetails BookTaxi([FromBody] SourceDestination sourceDestinationDto)
        {
            var taxi = _taxiBookingService.GetTaxi(sourceDestinationDto.Source, sourceDestinationDto.Destination);
            var ride = taxi.rides.Find(x => x.RideStatus == RideStatus.ArrivingToPickupLocation);
            var totalTimeForJourney = ride.TimeToReachToSource + ride.EstimatedRideTime;

            return new BookingDetails()
            {
                //generating customer on the fly, not creating customer module, as it is not part of bare metal requirement of this solution.
                CustomerId = new Customer().CustomerId,
                TaxiId = taxi.TaxiId,
                TotalTimeForJourney = totalTimeForJourney
            };
        }

        [HttpPost]
        [Route("api/tick/{offset}")]
        public string IncrementClock(int offset = 1)
        {
            var timeTracker = SimpleTimeTracker<int>.Instance;
            timeTracker.IncrementTime(offset);
            
            return "currentTimeValue " + timeTracker.CurrentTime;
        }

        [HttpPost]
        [Route("api/reset")]
        public string ResetSystem()
        {
            TaxiFleet.Instance.ResetTaxis();
            var timeTracker = SimpleTimeTracker<int>.Instance;
            timeTracker.ResetTime();
            return "System Resetted to original state";
        }
    }
}
