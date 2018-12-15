namespace TaxiBookingSystemCommons
{
    public static class SingletonFactory<T> where T: class
    {
        private static T instance = null;
        public static T getInstance(object padlock)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new  T();
                    }
                }
            }
            return instance;
            
        }
        
    }
}
