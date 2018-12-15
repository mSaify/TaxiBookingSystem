namespace TaxiBookingSystem.DTO
{

    //Considering each api call as a new customer, Currently not building module related to Customer
    public class Customer
    {
        private static int customerId=1;

        private object lockObj = new object();
       

        public Customer()
        {
            lock (lockObj)
            {
                this.CustomerId = customerId++;
            }
        }

        public int CustomerId { get; private set; }
        public string CustomerName { get; set; } 
    }
}
