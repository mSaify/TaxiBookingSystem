using System;

namespace TaxiBookingSystemEntities
{
    public class Ride<TimeType> : ITimeChangeSubscriber<int>, IDisposable
    {
        public int StartTime { get; private set; }

        public int EndTime { get; }

        public int TimeLeftToComplete { get; private set; }

        public int EstimatedRideTime { get; private set; }

        public int TimeToReachToSource { get; private set; }

        public int TimeLeftToReachSource  { get; private set; }

        public event EventHandler<RideStatusEventArgs> RideCompleted, RideStarted;
        
        public Source RideSource { get; }

        public Destination RideDestination { get; }

        public RideStatus RideStatus { get; private set; }

        public Taxi Taxi { get; }

        public bool OnGoing { get { return RideStatus == RideStatus.OnGoing ? true : false; } }

        public ITimeChangePublisher<int> TimeChangePublisher { get; private set; }

        public Ride(Source source, Destination destination)
        {
            RideSource = source;
            RideDestination = destination;
            
        }

        public Ride(Taxi taxi,Source source, Destination destination)
        {
            RideSource = source;
            RideDestination = destination;
            RideStatus = RideStatus.ArrivingToPickupLocation;
            TimeLeftToComplete = EstimatedRideTime = Helper.CalculateDistance(RideSource, RideDestination);
            TimeLeftToReachSource = TimeToReachToSource =  Helper.CalculateDistance(taxi.TaxiLocation, RideSource);
            Taxi = taxi;
            RideCompleted += OnRideCompleting;
            RideStarted += OnRideStarting;

        }
        
        public bool StartRide()
        {
            StartTime = SimpleTimeTracker<TimeType>.Instance.CurrentTime;
            //check conditions to begin ride
            return true;
        }

        public bool EndRide(Taxi taxi, DateTime startTime)
        {
            //check conditions to begin ride
            return true;
        }

        public void Subscriber(ITimeChangePublisher<int> timeChangePublisher)
        {
            TimeChangePublisher = timeChangePublisher;
            TimeChangePublisher.TimeChanged += TimeChangeHandler;
        }
  
        //handlers can be anything for simplicity I am adding it Ride class
        public void TimeChangeHandler(object sender, TimeEventArgs<int> eArgs)
        {

            if(this.RideStatus == RideStatus.OnGoing)
            {

                TimeLeftToComplete = TimeLeftToComplete - (eArgs.OffsetTimeChange);
                CheckIfRideCompleted();

            }
            else  if(this.Taxi.status == TaxiStatus.Booked && this.RideStatus == RideStatus.ArrivingToPickupLocation)
            {
                TimeLeftToReachSource = TimeLeftToReachSource - (eArgs.OffsetTimeChange);
                
                 if (TimeLeftToReachSource<=0) {

                    RideStarted(this, new RideStatusEventArgs(RideStatus.Started));

                    TimeLeftToComplete = TimeLeftToComplete + TimeLeftToReachSource;
                    CheckIfRideCompleted();
                }
            }
            
        }

        private void CheckIfRideCompleted()
        {
            if (this.TimeLeftToComplete <= 0)
                RideCompleted(this, new RideStatusEventArgs(RideStatus.Completed));
            else
                RideStatus = RideStatus.OnGoing;
        }

        private void OnRideCompleting(object sender, RideStatusEventArgs e)
        {
            
            if(e.CurrentRidestatus == RideStatus.Completed)
            {
                Taxi.TaxiLocation = RideDestination;
                RideStatus = RideStatus.Completed;
                this.RideCompleted -= OnRideCompleting;
                this.RideStarted -= OnRideStarting;
                this.TimeChangePublisher.TimeChanged -= TimeChangeHandler;
            }
        }

        private void OnRideStarting(object sender, RideStatusEventArgs e)
        {

            if (e.CurrentRidestatus == RideStatus.Started)
            {
                Taxi.TaxiLocation = RideSource;
                RideStatus = RideStatus.OnGoing;
            }
        }

     
        //Implement IDisposable.
        public void Dispose()
        {
            this.RideCompleted -= OnRideCompleting;
            this.RideStarted -= OnRideStarting;
            this.TimeChangePublisher.TimeChanged -= TimeChangeHandler;

            this.RideCompleted = null;
            this.RideStarted = null;
            this.TimeChangePublisher = null;

        }
        
    }
}