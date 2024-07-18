using MongoDB.Bson;

namespace TrackOrders.ViewModels
{
    public class DeliveryResponse
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public bool HasDelivered { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}
