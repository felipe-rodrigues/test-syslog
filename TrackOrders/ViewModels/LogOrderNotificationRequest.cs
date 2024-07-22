namespace TrackOrders.ViewModels
{
    public class LogOrderNotificationRequest
    {
        public string OrderNumber { get; set; }
        public string Message { get; set; }
        public int? Attempt { get; set; }
        public bool HasDelivered { get; set; }
        public bool IsNewOrderNotification { get; set; }
    }
}
