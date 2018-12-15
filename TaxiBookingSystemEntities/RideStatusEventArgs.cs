using System;

namespace TaxiBookingSystemEntities
{
    public class RideStatusEventArgs : EventArgs
    {
        public RideStatusEventArgs(RideStatus rideStatus)
        {
            CurrentRidestatus = rideStatus;
        }
        public RideStatus CurrentRidestatus { get; }
    }
}
