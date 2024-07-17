using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
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

        public OrderController(TrackOrdersContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderRequest request)
        {

            var order = _mapper.Map<Order>(request);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<OrderResponse>(order);

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _context.Orders.ToListAsync();

            return Ok(orders);
        }

    }
}
