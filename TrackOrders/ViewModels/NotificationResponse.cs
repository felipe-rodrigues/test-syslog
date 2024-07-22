namespace TrackOrders.ViewModels
{
    public class NotificationResponse
    {
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public string OrderNumber { get; set; }
        public int? Attempt { get; set; }
        public bool Viewed { get; set; }
        public DateTime? ViewedDate { get; set; }
        public bool IsNewOrderNotification { get; set; }
    }
}
