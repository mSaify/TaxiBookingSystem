using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxiBookingSystemCommons;
using TaxiBookingSystemEntities;

namespace TaxiBookingSystem.DTO
{
    public class TaxiDetailDto
    {
        public int TaxiId { get; set; }

        public Source LastestRideSource { get; set; }


        //This will the last location of taxi not with respect to change in time,
        //the location of taxi will change only when ridestatus changes.
        public Location LastTaxiLocation { get; set; }

        public Destination LastestRideDestination { get; set; }

        public String LastestRideStatus { get; set; }

        public string TaxiStatus { get; set; }

        private Taxi _taxi;

        public TaxiDetailDto(Taxi taxi)
        {
            _taxi = taxi;
        }

        public TaxiDetailDto ConvertToDto()
        {
            this.TaxiId = _taxi.TaxiId;
            Ride<int> ride = _taxi.rides.LastOrDefault();

            if (ride != null) {
                this.LastestRideSource = ride.RideSource;
                this.LastestRideDestination = ride.RideDestination;
                LastestRideStatus = ride.RideStatus.ToDescription();
            }

            this.TaxiStatus = _taxi.status.ToDescription();
            this.LastTaxiLocation = _taxi.TaxiLocation;

            return this;
        }
    }
}
