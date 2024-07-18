using System.Security.Claims;
using TrackOrders.Data.Entities;
using TrackOrders.ViewModels;

namespace TrackOrders.Configuration
{
    public static class HttpContextExtnsions
    {

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var stringOrg = user.FindFirst("UserId").Value;

            return stringOrg;
        }

        public static UserResponse GetUserReference(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst("UserId").Value;
            var name = user.FindFirst(ClaimTypes.Name).Value;
            var email = user.FindFirst(ClaimTypes.Email).Value;

            return new UserResponse
            {
                Name = name,
                Email = email,
                Id = userId
            };

        }
    }
}
