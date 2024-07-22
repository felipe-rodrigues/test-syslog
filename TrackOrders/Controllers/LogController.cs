using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackOrders.Configuration;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        private readonly TrackOrdersContext _context;

        public LogController(TrackOrdersContext context)
        {
            _context = context;
        }

        [HttpPost("order/{orderNumber}/viewed")]
        public async Task<IActionResult> AddOrderNotificationViewed(string orderNumber, [FromBody] LogOrderNotificationRequest request)
        {
            var currenUser = User.GetUserReference();

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if (order == null)
                return BadRequest("Pedido não encontrado");

            var logNotification = new NotificationLog()
            {
                CreatedDate = DateTime.Now,
                HasDelivered = request.HasDelivered,
                OrderId = order.Id.ToString(),
                OrderNumber = orderNumber,
                ViewerId = currenUser.Id,
                ViewerEmail = currenUser.Email,
                Message  = request.Message,
                IsNewOrderNotification = request.IsNewOrderNotification,
                Attempt = request.Attempt,
            };

            _context.NotificationLogs.Add(logNotification);

            await _context.SaveChangesAsync();

            return Ok();

        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allOrders = await _context.Orders.ToListAsync();

            var currenUser = User.GetUserReference();
            var viewedNotificationsByUser = await _context.NotificationLogs.Where(n => n.ViewerId == currenUser.Id).ToListAsync();  

            return Ok(GetAllNotifications(allOrders,viewedNotificationsByUser));
        }


        private List<NotificationResponse> GetAllNotifications(List<Order> allOrders, List<NotificationLog> vieweNotificationsByUser)
        {

            //TODO: mover lógica para um serviço

            var listOfNotifications = new List<NotificationResponse>();

            foreach (var order in allOrders)
            {

                var orderNotifications = vieweNotificationsByUser.Where(n => n.OrderNumber == order.Number).ToList();

                var orderCreationNotification = new NotificationResponse()
                {
                    DateTime = order.CreatedDate,
                    Message = $"Pedido {order.Number} criado",
                    OrderNumber = order.Number,
                    IsNewOrderNotification = true
                };

                var viewNotification = orderNotifications.FirstOrDefault(n => n.IsNewOrderNotification);
                if (viewNotification != null)
                {
                    orderCreationNotification.Viewed = true;
                    orderCreationNotification.ViewedDate = viewNotification.CreatedDate;
                }

                listOfNotifications.Add(orderCreationNotification);

                if (order.Deliveries == null)
                    continue;

                foreach (var delivery in order.Deliveries)
                {
                    var message = delivery.HasDelivered ? $"Entrega do pedido {order.Number} com sucesso" : $"Tentativa {delivery.Attempt} de entrega do pedido {order.Number}";
                    var deliveryNotification = new NotificationResponse()
                    {
                        DateTime = delivery.DateTime,
                        Attempt = delivery.Attempt,
                        OrderNumber = order.Number,
                        Message = message
                    };

                    var viewedDeliveryNotification = orderNotifications.FirstOrDefault(n => n.Attempt == delivery.Attempt);
                    if (viewedDeliveryNotification != null)
                    {
                        deliveryNotification.Viewed = true;
                        deliveryNotification.ViewedDate = viewedDeliveryNotification.CreatedDate;
                    }

                    listOfNotifications.Add(deliveryNotification);
                }

            }

            return listOfNotifications;
        }


    }
}
