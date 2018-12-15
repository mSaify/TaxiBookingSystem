using System;

namespace TaxiBookingSystemEntities
{
    public interface ITimeChangePublisher<T>
    {
        event EventHandler<TimeEventArgs<T>> TimeChanged;

        void Publish(T newTime, T offsetChangeTime);
    }
}
