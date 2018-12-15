using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TaxiBookingSystemEntities
{
 
    public enum TaxiStatus
    {
        [Description("Taxi Is Available")]
        Available,
        [Description("Taxi Is Booked")]
        Booked,
        [Description("Taxi Travelling Towards Drop Location")]
        FinishingRide,
        [Description("Taxi Reached PickUp Point")]
        ReachedPickUpPoint
    };

    public class Taxi
    {
        //static id , all id's will be increment of this.
        static int taxiId= 1;

        static object IdlockObject;

        public int TaxiId { get;  }

        public string Operator { get; }

        public string Driver { get; set; }

        public decimal BasePice { get; }

        public decimal PerMilesCharges { get; private set; }

        public TaxiStatus status { get; private set; }

        public List<Ride<int>> rides { get; } = new List<Ride<int>>();
        
        static Taxi()
        {
            IdlockObject = new object();
        }

        public void ClearAllRides()
        {

            rides.ForEach(x => x.Dispose());
            rides.Clear();
        }

        public void ResetTaxiStatus()
        {
            this.status = TaxiStatus.Available;
        }

        public Taxi()
        {
            lock(IdlockObject)
            {
                TaxiId = taxiId;
                taxiId++;
            }
        }

        

        public Location TaxiLocation { get; set; } = new Location();
  
        public Taxi(string model, decimal basePrice, decimal perMilesCharges)
        {
            BasePice = basePrice;
            PerMilesCharges = perMilesCharges;
        }

        public virtual decimal CalculateFare(decimal traveledKms)
        {
            return BasePice + (traveledKms * PerMilesCharges);
        }
        
        public bool BookTaxi(Source source, Destination destination)
        {
            
            if(this.status == TaxiStatus.Available)
            {
                this.status = TaxiStatus.Booked;
                var ride = new Ride<int>(this, source, destination);
                ride.Subscriber(SimpleTimeTracker<int>.Instance.TimeChangePublisher);
                ride.RideCompleted += OnRideCompleting;
                ride.RideStarted += OnRideStarting;
                rides.Add(ride);
                return true;
            }

            return false;
        }

        private void OnRideStarting(object sender, RideStatusEventArgs e)
        {
            if (e.CurrentRidestatus == RideStatus.Started)
            {
                status = TaxiStatus.ReachedPickUpPoint; //later we can see whether this state transition is really required
                status = TaxiStatus.FinishingRide;
                
            }
        }

        private void OnRideCompleting(object sender, RideStatusEventArgs e)
        {
            if (e.CurrentRidestatus == RideStatus.Completed)
            {
                status = TaxiStatus.Available;
            }
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }

    public class PrimeTaxi : Taxi
    {
        public PrimeTaxi()
        {

        }
        public string Model { get; }

        public PrimeTaxi(string model, decimal basePrice, decimal perMilesCharges)
            : base(model, basePrice, perMilesCharges)
        {
        }
    }
    public class EconomicalTaxi : Taxi
    {
        public string Model { get; }

        public EconomicalTaxi()
        {

        }

        public EconomicalTaxi(string model, decimal basePrice, decimal perMilesCharges)
            : base(model, basePrice, perMilesCharges)
        {
        }
    }

    public class Driver
    {

    }
}
