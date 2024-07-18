using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackOrders.Configuration;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;

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
        public async Task<IActionResult> AddNotificationViewed(string orderNumber, [FromQuery] bool delivered = false)
        {
            var currenUser = User.GetUserReference();

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if (order == null)
                return BadRequest("Pedido não encontrado");

            var logNotification = new NotificationLog()
            {
                CreatedDate = DateTime.Now,
                HasDelivered = delivered,
                OrderId = order.Id.ToString(),
                OrderNumber = orderNumber,
                ViewerId = currenUser.Id,
                ViewerEmail = currenUser.Email,
            };

            _context.NotificationLogs.Add(logNotification);

            await _context.SaveChangesAsync();

            return Ok();

        }

    }
}
