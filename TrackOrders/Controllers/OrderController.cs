using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
using TrackOrders.Data.ValueObjects;
using TrackOrders.Hubs;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly TrackOrdersContext _context;
        private readonly IHubContext<NotificationHub> _hub;

        public OrderController(TrackOrdersContext context, IMapper mapper, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _mapper = mapper;
            _hub = hub;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderRequest request)
        {

            var order = _mapper.Map<Order>(request);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync(GlobalConstants.ORDER_EVENT_NAME, $"Pedido {order.Number} criado");

            var response = _mapper.Map<OrderResponse>(order);

            return Ok(response);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var orders = await _context.Orders.ToListAsync();

            var listResponse = _mapper.Map<List<Order>, List<OrderResponse>>(orders);

            return Ok(listResponse);
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetByOrderNumber(string orderNumber)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if (order == null)
                return BadRequest("Pedido não encontrado");

            var response = _mapper.Map<OrderResponse>(order);
            
            return Ok(response);
        }


        [HttpPut("{orderNumber}/delivery")]
        public async Task<IActionResult> AddDeliveryAttempt(string orderNumber, [FromQuery] bool delivered = true)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Number.ToUpper() == orderNumber.ToUpper());

            if (order == null)
                return BadRequest("Pedido não encontrado");

            if (order.Deliveries == null)
                order.Deliveries = new List<Delivery>();


            var delivery = new Delivery()
            {
                Attempt = order.Deliveries.Count + 1,
                DateTime = DateTime.Now,
                HasDelivered = delivered
            };

            order.HasDelivered = delivered;
            order.Deliveries.Add(delivery);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();


            await _hub.Clients.All.SendAsync(GlobalConstants.ORDER_EVENT_NAME, $"Tentativa {delivery.Attempt} com {(delivered ? "sucesso" : "falha")}");

            return Ok(delivery);
        }

    }
}
