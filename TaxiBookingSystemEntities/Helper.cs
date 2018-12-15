using System;

namespace TaxiBookingSystemEntities
{
    public static class Helper
    {
        public static int CalculateDistance(Location start, Location end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }
    }
    
}
