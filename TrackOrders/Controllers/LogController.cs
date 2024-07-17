using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackOrders.Data.Context;

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

        [HttpPost("order/{orderId}/viewed")]
        public async Task<IActionResult> AddNotificationViewed(string orderId , [FromQuery] bool delivered = false)
        {


            return Ok();

        }

    }
}
