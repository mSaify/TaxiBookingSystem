using System;
using System.Collections.Generic;
using TaxiBookingSystemCommons;

namespace TaxiBookingSystemEntities
{
    public sealed class WorldMap
    {
        static Dictionary<string, City> cities;
        public WorldMap()
        {
        }

        public WorldMap withOnly2DCities()
        {
            //Assume that cities are fetched from database and list of cities are generated as Dictionary
            cities = new Dictionary<string, City>()
            {
                { "Manhattan", new City("Manhattan") },
                { "Singapore", new City("Singapore") },
            };
            
            return this;
        }

        public City GetCity(string cityName)
        {
            return cities[cityName];
        }
        
        private static readonly object padlock = new object();
        public static WorldMap Instance => SingletonFactory<WorldMap>.getInstance(padlock);
    }
   

    public class City
    {
        public String Name { get; }

        public City(string name)
        {
            Name = name;
        }

        public bool WithinCityLocation(Source source, Destination destination)
        {
            //as assigned value is max integer we don't have check whether source destination is within boundary

            return true;
        }
    }
}
