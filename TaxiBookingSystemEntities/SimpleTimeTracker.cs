using System;
using TaxiBookingSystemCommons;

namespace TaxiBookingSystemEntities
{
    //There can be different object type of time tracker, I am just using simple Integer counter time tracker.
    public  class SimpleTimeTracker<T> : ITimeTracker<T>
    {
        private static readonly object padlock = new object();
        private T epochTime;
        private T currentTime;

        public ITimeChangePublisher<T> TimeChangePublisher;
        
        public static SimpleTimeTracker<int> Instance =>  SingletonFactory<SimpleTimeTracker<int>>.getInstance(padlock);
        public  T EpochTime => epochTime;
        public T CurrentTime => currentTime;
        ITimeChangePublisher<T> ITimeTracker<T>.TimeChangePublisher { get; }

        public SimpleTimeTracker()
        {
            TimeChangePublisher = new TimeChangePublisher<T>();
        }
        public void IncrementTime()
        {
            Instance.currentTime++;
            Instance.TimeChangePublisher.Publish(Instance.currentTime,1);
        }

        public void ResetTime()
        {
            Instance.currentTime = 0 ;
            Instance.epochTime = 0;
        }

        public void IncrementTime(int offset)
        {
            Instance.currentTime= Instance.currentTime + offset;
            Instance.TimeChangePublisher.Publish(Instance.currentTime, offset);
        }
    }
    
    public class TimeEventArgs<T> : EventArgs
    {
        public T CurrentTime { get; set; }

        public T OffsetTimeChange { get; set; }
    }
}
