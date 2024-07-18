using MongoDB.Bson;
using TrackOrders.Data.ValueObjects;

namespace TrackOrders.Data.Entities
{
    public class NotificationLog
    {
        public ObjectId Id { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ViewerId { get; set; }
        public string ViewerEmail { get; set; }

    }
}
