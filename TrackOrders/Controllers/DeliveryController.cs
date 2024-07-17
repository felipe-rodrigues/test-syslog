using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryController : ControllerBase
    {

        private readonly TrackOrdersContext _context;

        public DeliveryController(TrackOrdersContext context)
        {
            _context = context;
        }

        [HttpPost("{orderNumber}")]
        public async Task<IActionResult> SaveDelivery(string orderNumber)
        {
            
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if(order == null)
            {
                return BadRequest("Pedido não encontrado");
            }

            var delivery = new Delivery()
            {
                DeliveredAt = DateTime.Now,
                OrderNumber = orderNumber
            };

            order.HasDelivered = true;
            _context.Deliveries.Add(delivery);

            await _context.SaveChangesAsync();
            return Ok(delivery);
        }

    }
}
