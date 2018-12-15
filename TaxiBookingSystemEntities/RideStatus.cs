using System.ComponentModel;

namespace TaxiBookingSystemEntities
{
    public enum RideStatus
    {
        [Description("Arriving To Pickup Location")]
        ArrivingToPickupLocation,
        [Description("Ride Started")]
        Started,
        [Description("OnWay To Drop Location")]
        OnGoing,
        [Description("Ride Completed")]
        Completed,
        [Description("Ride Canceled")]
        Canceled,
        [Description("Ride Delayed")]
        Delayed
    }
}
