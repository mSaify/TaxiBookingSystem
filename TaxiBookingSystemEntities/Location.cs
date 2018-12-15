using System;

namespace TaxiBookingSystemEntities
{
    public class Location : IEquatable<Location>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Location()
        {

        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        public bool Equals(Location other)
        {
            if (other == null)
                return false;

            return
                (object.ReferenceEquals(this.X, other.X) ||
                this.X.Equals(other.X) ) &&
                (object.ReferenceEquals(this.Y, other.Y) ||
                this.Y.Equals(other.Y));
        }
    }
    
}
