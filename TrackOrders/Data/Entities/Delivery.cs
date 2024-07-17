using MongoDB.Bson;

namespace TrackOrders.Data.Entities
{
    public class Delivery
    {
        public ObjectId Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}
