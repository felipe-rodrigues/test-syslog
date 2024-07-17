using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackOrders.Services.Interfaces;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IPasswordHashService _passwordHashService;

        public AuthController(IAuthService authService, IPasswordHashService passwordHashService)
        {
            _authService = authService;
            _passwordHashService = passwordHashService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Auth(UserLoginRequest request)
        {
            try
            {
                var response = await _authService.AuthenticateUser(request.Email, request.Password);
                if (response.user != null)
                {
                    return Ok(new UserLoginResponse
                    {
                        Email = response.user.Email,
                        Token = response.token,
                        Id = response.user.Id.ToString(),
                    });
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest("User not found");
            }

        }

    }
}
