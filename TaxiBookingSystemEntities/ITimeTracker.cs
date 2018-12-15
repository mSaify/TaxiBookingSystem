using System;
using System.Collections.Generic;
using System.Text;

namespace TaxiBookingSystemEntities
{
    interface ITimeTracker<T>
    {
        T EpochTime { get; }
        void IncrementTime();

        T CurrentTime { get; }

        ITimeChangePublisher<T> TimeChangePublisher { get; }
        
    }
}
