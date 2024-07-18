using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryController : ControllerBase
    {

        private readonly TrackOrdersContext _context;
        private readonly IMapper _mapper;

        public DeliveryController(TrackOrdersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("{orderNumber}")]
        public async Task<IActionResult> SaveDelivery(string orderNumber, [FromQuery] bool delivered = true)
        {
            
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if(order == null)
            {
                return BadRequest("Pedido não encontrado");
            }

            var delivery = new Delivery()
            {
                ExecutedAt = DateTime.Now,
                OrderNumber = orderNumber,
                HasDelivered = delivered
            };

            if (delivered)
            {
                //TODO: send notifications
                order.HasDelivered = true;
                _context.Orders.Update(order);
            }
            _context.Deliveries.Add(delivery);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<DeliveryResponse>(delivery);

            return Ok(response);
        }

    }
}
