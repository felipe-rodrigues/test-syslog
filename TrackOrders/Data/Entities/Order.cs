using MongoDB.Bson;
using TrackOrders.Data.ValueObjects;

namespace TrackOrders.Data.Entities
{
    public class Order
    {
        public ObjectId Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public Address DeliveryAddress { get; set; }
        //TODO: talvez alterar para enum , se considerar mais casos além de entregue ou não entregue
        public bool HasDelivered { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
