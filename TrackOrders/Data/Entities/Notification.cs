using MongoDB.Bson;
using TrackOrders.Data.ValueObjects;

namespace TrackOrders.Data.Entities
{

    //TODO: por agora salvando apenas log de notificações, se necessário tornar mais gloal salvando referencia de outros logs da aplicação também
    public class NotificationLog
    {
        public ObjectId Id { get; set; }
        public int? Attempt { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ViewerId { get; set; }
        public string ViewerEmail { get; set; }
        public string Message { get; set; }
        public bool IsNewOrderNotification { get; set; }

    }
}
