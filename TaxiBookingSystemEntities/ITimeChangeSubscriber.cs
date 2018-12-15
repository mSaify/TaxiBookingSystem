namespace TaxiBookingSystemEntities
{
    public interface ITimeChangeSubscriber<T>
    {
        void Subscriber(ITimeChangePublisher<T> timeChangePublisher);
        void TimeChangeHandler(object sender, TimeEventArgs<T> eArgs);

        ITimeChangePublisher<T> TimeChangePublisher { get;  }

    }
}
