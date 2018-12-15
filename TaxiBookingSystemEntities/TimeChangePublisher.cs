using Microsoft.Extensions.Logging;
using System;
using TaxiBookingSystemCommons;

namespace TaxiBookingSystemEntities
{
    public sealed class TimeChangePublisher<T> : ITimeChangePublisher<T>
    {
        public event EventHandler<TimeEventArgs<T>> TimeChanged;

        ILogger _logger = ApplicationLogging.LoggerFactory.CreateLogger(nameof(TaxiFleet));

            TimeEventArgs<T> args = new TimeEventArgs<T>();

        public TimeChangePublisher()
        {

        }

        public void Publish(T newTime, T offsetChange)
        {
            args.CurrentTime = newTime;
            args.OffsetTimeChange = offsetChange;
            if (TimeChanged == null)
                _logger.LogError("There are no active Rides, Time event will not have any effect");
            else
            TimeChanged(this, args);

        }

    }
}
