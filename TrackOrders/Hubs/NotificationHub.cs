using Microsoft.AspNetCore.SignalR;

namespace TrackOrders.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync(GlobalConstants.ORDER_EVENT_NAME, message);
        }
    }
}
