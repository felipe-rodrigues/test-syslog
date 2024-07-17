using MongoDB.Bson;
using TrackOrders.Data.ValueObjects;

namespace TrackOrders.ViewModels
{
    public class OrderResponse
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public Address DeliveryAddress { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
