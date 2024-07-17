using MongoDB.Bson;

namespace TrackOrders.Data.Entities
{
    public class NotificationLog
    {
        public ObjectId Id { get; set; }
        public string OrderId { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ViewerId { get; set; }

    }
}
