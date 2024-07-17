using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using TrackOrders.Data.Context;
using TrackOrders.Data.Entities;
using TrackOrders.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace TrackOrders.Services.Implementation
{
    public class AuthService : IAuthService
    {

        private readonly TrackOrdersContext _context;
        private readonly IPasswordHashService _passwordHashService;
        private readonly AppSettings _settings;

        public AuthService(TrackOrdersContext context, IPasswordHashService passwordHashService, IOptions<AppSettings> settings)
        {
            _context = context;
            _passwordHashService = passwordHashService;
            _settings = settings.Value;
        }

        public async Task<(
            User user, string token)> AuthenticateUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user != null)
            {
                var checkPwd = _passwordHashService.Check(user.Password, password);
                if (checkPwd.verified)
                {
                    var token = await GenerateToken(user);
                    return (user, token);
                }
                else
                {
                    throw new Exception("Needs upgrade password");
                }
            }
            else
            {
                throw new Exception("User not found exception");
            }
        }

        public async Task<string> GenerateToken(User user)
        {
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Jwt.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenGenerated = tokenHandler.WriteToken(token);

                return await Task.FromResult(tokenGenerated);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
