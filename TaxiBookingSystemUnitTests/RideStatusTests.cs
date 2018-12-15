using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaxiBookingSystemCore;
using TaxiBookingSystemEntities;

namespace TaxiBookingSystemCoreUnitTests
{
    [TestClass]
    public class RideStatusTests
    {
        [TestInitialize]
        public void InitializeTestSuite()
        {

            WorldMap worldMap = WorldMap.Instance.withOnly2DCities();
            TaxiFleet taxiFleet = TaxiFleet.Instance;
            SimpleTimeTracker<int> timetracker = SimpleTimeTracker<int>.Instance;

            //reseting taxi source location
            TaxiFleet.Instance.ResetTaxis();

        }

        [TestMethod]
        [DataRow(0, 1, 4, 2, new int[] { 0, 1, 3, 5 })]
        //0 is epochTime
        //1 is where ride reached source
        //3 on going ride time
        //5 when ride reached destination
        [DataRow(1000, 1, 10000, 2, new int[] { 0, 1001, 3000, 9001 })]
        public void BookACab_CheckAfterTicks_RideStatus(int sx, int sy,
                                                        int dx, int dy, int[] ticks)
        {

            var taxiBookingService = new TaxiBookingService();
            var rand = new Random();
            var source = new Source(sx, sy);
            var destination = new Destination(dx, dy);

            var taxi = taxiBookingService.GetTaxi(source,
                                                destination);
            
            Assert.IsNotNull(taxi);
            
            var diffTimes = new TimeAtDifferentPoints();
            diffTimes.StartTime = ticks[0];
            diffTimes.SourceReachedTime = ticks[1];
            diffTimes.HalfJourneyTime = ticks[2];
            diffTimes.ReachedDestinationTime = ticks[3];

            Assert.AreEqual(taxi.status, TaxiStatus.Booked);

            //Increase Time to Reach to source
            SimpleTimeTracker<int>.Instance.IncrementTime(diffTimes.SourceReachedTime);
            Assert.IsTrue(taxi.TaxiLocation.Equals(source));
            Assert.AreEqual(taxi.status, TaxiStatus.FinishingRide);


            //Increase Time to Reach to midway of journey
            SimpleTimeTracker<int>.Instance.IncrementTime(diffTimes.HalfJourneyTime- diffTimes.SourceReachedTime);
            //Assert.AreEqual(taxi.rides.Count, 1);
            Assert.IsTrue(taxi.rides.Exists(x => x.OnGoing == true));


            //Increase Time to Reach to end of Journey
            SimpleTimeTracker<int>.Instance
                .IncrementTime(diffTimes.SourceReachedTime+diffTimes.ReachedDestinationTime- diffTimes.HalfJourneyTime);
            Assert.IsTrue(taxi.rides.Exists(x => x.OnGoing == false));
            Assert.IsTrue(taxi.TaxiLocation.Equals(destination));
            Assert.AreEqual(taxi.status, TaxiStatus.Available);
        }

        
        [TestMethod]
        public void BookMultipleCabs_CheckAfterTicks_RideStatus()
        {

            var source1 = new Source(0, 1); var destination1 = new Destination(4, 2);
            var source2 = new Source(2, 0); var destination2 = new Destination(3, 5);

            var taxi1 =BookACab(source1, destination1);
            var taxi2 = BookACab(source2, destination2);

            Assert.AreNotEqual(taxi1, taxi2);
            Assert.AreEqual(taxi1.status, TaxiStatus.Booked);
            Assert.AreEqual(taxi2.status, TaxiStatus.Booked);
            
            var diffTimeForTaxi1 = new TimeAtDifferentPoints()
                                    .SetTimeAtDifferentPoints(new int[] { 0, 1, 3, 5 });

            var diffTimeForTaxi2 = new TimeAtDifferentPoints()
                                    .SetTimeAtDifferentPoints(new int[] { 0, 2, 3, 6 });



            //Increase Time to Reach to source
            SimpleTimeTracker<int>.Instance.IncrementTime(diffTimeForTaxi2.SourceReachedTime);

            Assert.IsTrue(taxi1.TaxiLocation.Equals(source1));
            Assert.IsTrue(taxi2.TaxiLocation.Equals(source2));
            Assert.AreEqual(taxi1.status, TaxiStatus.FinishingRide);
            Assert.AreEqual(taxi2.status, TaxiStatus.FinishingRide);
            
            //Increase Time to Reach to midway of journey
            SimpleTimeTracker<int>.Instance.IncrementTime(diffTimeForTaxi2.HalfJourneyTime - diffTimeForTaxi2.SourceReachedTime);
            //Assert.AreEqual(taxi.rides.Count, 1);
            Assert.IsTrue(taxi1.rides.Exists(x => x.OnGoing == true));
            Assert.IsTrue(taxi2.rides.Exists(x => x.OnGoing == true));


            //Increase Time to Reach to end of Journey for taxi1
            SimpleTimeTracker<int>.Instance
                .IncrementTime(diffTimeForTaxi1.SourceReachedTime + 
                               diffTimeForTaxi1.ReachedDestinationTime - 
                               diffTimeForTaxi1.HalfJourneyTime + 1);
            
            Assert.IsTrue(taxi1.rides.Exists(x => x.OnGoing == false));
            Assert.IsTrue(taxi1.TaxiLocation.Equals(destination1));
            Assert.AreEqual(taxi1.status, TaxiStatus.Available);

            //For taxi2 time need to be incremented by one more tick to finish ride
            SimpleTimeTracker<int>.Instance.IncrementTime();

            Assert.IsTrue(taxi2.rides.Exists(x => x.OnGoing == false));
            Assert.IsTrue(taxi2.TaxiLocation.Equals(destination2));
            Assert.AreEqual(taxi2.status, TaxiStatus.Available);
        }
        

        public Taxi BookACab(Source source, Destination destination)
        {
            var taxiBookingService = new TaxiBookingService();

            return taxiBookingService.GetTaxi(source,
                                                destination);
        }
    }
    
    public class TimeAtDifferentPoints
    {
        public int StartTime { get; set; }
        public int SourceReachedTime { get; set; }
        public int HalfJourneyTime { get; set; }

        public int ReachedDestinationTime { get; set; }

        public TimeAtDifferentPoints SetTimeAtDifferentPoints(int[] ticks)
        {
            StartTime = ticks[0];
            SourceReachedTime = ticks[1];
            HalfJourneyTime = ticks[2];
            ReachedDestinationTime = ticks[3];
            return this;
        }
    }
    
}
