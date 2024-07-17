using Amazon.Util;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
using TrackOrders.Services.Interfaces;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly TrackOrdersContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHashService _passwordHashService;

        public UserController(TrackOrdersContext context, IMapper mapper, IPasswordHashService passwordHashService)
        {
            _context = context;
            _mapper = mapper;
            _passwordHashService = passwordHashService;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserRequest request)
        {

            var user = _mapper.Map<User>(request);
            user.Password = _passwordHashService.Hash(user.Password);
            _context.Users.Add(user);
            
            await _context.SaveChangesAsync();

            var response = _mapper.Map<UserResponse>(user);

            return Ok(response);
        }
    }
}
