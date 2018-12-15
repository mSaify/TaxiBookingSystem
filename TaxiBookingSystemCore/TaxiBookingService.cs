using TaxiBookingSystemEntities;

namespace TaxiBookingSystemCore
{
    public class TaxiBookingService
    {
        private object lockObject;

        public TaxiBookingService()
        {
            lockObject = new object();
        }

        public Taxi GetTaxi(Source source, Destination destination)
        {
            var city = WorldMap.Instance.GetCity("Manhattan");
            
            //though there is no city concept, prehaps later we can define cities and have boundariess
            //for now int limit is boundary for any city.
            if (city.WithinCityLocation(source, destination))
            {
                //not the best way to synchronize, but the system can be used at same time by multiple threads
                //to avoid booking of same cab at same time for two different users.
                lock (lockObject)
                {
                    var alltaxis = TaxiFleet.Instance;
                    var taxi = alltaxis.FindAvailableTaxi(source);
                    if (taxi == null) return null;
                    taxi.BookTaxi(source, destination);
                    return taxi;
                }
            }

            return null;
        }
        
    }
}
