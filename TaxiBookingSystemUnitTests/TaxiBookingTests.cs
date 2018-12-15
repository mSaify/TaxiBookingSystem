using System;
using TaxiBookingSystemEntities;
using TaxiBookingSystemCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TaxiBookingSystemCoreTests
{
    [TestClass]
    public class TaxiBooking
    {
        [TestInitialize]
        public void InitializeTestSuite()
        {
            
            WorldMap worldMap = WorldMap.Instance.withOnly2DCities();
            TaxiFleet taxiFleet = TaxiFleet.Instance;
            SimpleTimeTracker<int> timetracker = SimpleTimeTracker<int>.Instance;

            TaxiFleet.Instance.ResetTaxis();

        }

        [TestMethod]
        public void BookACab_WhenAllCabs_AreAvailable()
        {

            var taxiBookingService = new TaxiBookingService();
            var rand = new Random();
            var taxi = taxiBookingService.GetTaxi(new Source(rand.Next(9), rand.Next(9)),
                                       new Destination(rand.Next(9), rand.Next(9)));

            Assert.IsNotNull(taxi);
            Assert.AreEqual(taxi.status, TaxiStatus.Booked);
        }

        [TestMethod]
        public void BookACab_WhenNoCab_IsAvailable()
        {

            var taxiBookingService = new TaxiBookingService();
            var rand = new Random();
            var taxi1 = taxiBookingService.GetTaxi(new Source(rand.Next(9), rand.Next(9)),
                                       new Destination(rand.Next(9), rand.Next(9)));
            var taxi2 = taxiBookingService.GetTaxi(new Source(rand.Next(9), rand.Next(9)),
                                       new Destination(rand.Next(9), rand.Next(9)));

            var taxi3 = taxiBookingService.GetTaxi(new Source(rand.Next(9), rand.Next(9)),
                                       new Destination(rand.Next(9), rand.Next(9)));

            var newTaxi = taxiBookingService.GetTaxi(new Source(rand.Next(9), rand.Next(9)),
                                       new Destination(rand.Next(9), rand.Next(9)));

            Assert.IsNull(newTaxi);

            TaxiFleet.Instance.ResetTaxis();

        }

        [TestMethod]
        public void BookANearestCab_WhenMultipleCabsAreAvailable()
        {

            var taxiBookingService = new TaxiBookingService();

            var source = new Source(0, 0); var destination = new Destination(3, 4);
            //get a taxi for a particular destination;
            var taxi1 = taxiBookingService.GetTaxi(source,destination);

            SimpleTimeTracker<int>.Instance.IncrementTime(7);

            //check whether taxi reached to destination
            Assert.AreEqual(taxi1.TaxiLocation, destination);
        
            //here distance from taxi1 to next source is 3
            //distnace from 0,0 to next source is 4
            //so taxi1 should be booked.
            var nextsource = new Source(1, 3);
            var nextdestination = new Destination(3, 5);
            var sametaxi = taxiBookingService.GetTaxi(nextsource, nextdestination);

            Assert.AreEqual(taxi1, sametaxi);
        }

    }
}
