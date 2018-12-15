using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using TaxiBookingSystemCommons;

namespace TaxiBookingSystemEntities
{
    public class TaxiFleet
    {
        private List<Taxi> alltaxis = new List< Taxi>();

        ILogger _logger;
        
        public TaxiFleet()
        {
            _logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(TaxiFleet));

            alltaxis.Add(new EconomicalTaxi());
            alltaxis.Add(new PrimeTaxi());
            alltaxis.Add(new PrimeTaxi());
        }

        public Taxi GetTaxi(int taxiId)
        {
            return alltaxis.Find(x => x.TaxiId == taxiId);
        }

        public void ResetTaxis()
        {
            alltaxis.ForEach(taxi => { taxi.TaxiLocation.X = 0; taxi.TaxiLocation.Y = 0; });
            alltaxis.ForEach(taxi => { taxi.ClearAllRides(); });
            alltaxis.ForEach(taxi => { taxi.ResetTaxiStatus(); });
        }

        private static readonly object padlock = new object();
        public static TaxiFleet Instance => SingletonFactory<TaxiFleet>.getInstance(padlock);

        public  Taxi FindAvailableTaxi(Source source)
        {
            if (!alltaxis.Exists(x => x.status == TaxiStatus.Available))
            {
                _logger.LogError("No taxi Available when customer x tried to book taxi");
                return null;
            }
            else
            {
                var minDistance = int.MaxValue;
                var currDistance = 0;
                Taxi availableTaxi=null;
                Taxi prevSelectedTaxi = null;
                foreach (var taxi in alltaxis.FindAll(x=>x.status == TaxiStatus.Available))
                {
                    currDistance =
                        Math.Abs(taxi.TaxiLocation.X - source.X) + 
                        Math.Abs(taxi.TaxiLocation.Y - source.Y);
                
                    if (currDistance < minDistance)
                    {
                        availableTaxi = taxi;
                        //selecting Taxi which has minimum taxiId
                        if (prevSelectedTaxi != null && availableTaxi.TaxiId > prevSelectedTaxi.TaxiId)
                        {
                            availableTaxi = prevSelectedTaxi;
                            prevSelectedTaxi = null;
                        }
                        minDistance = currDistance;
                    }
                }
                return availableTaxi;
            }
        }
         
    }
    
}
