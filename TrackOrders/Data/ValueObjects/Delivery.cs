namespace TrackOrders.Data.ValueObjects
{
    public class Delivery
    {
        public int Attempt { get; set; }
        public DateTime DateTime { get; set; }
        public bool HasDelivered { get; set; }

    }
}
