using TrackOrders.Data.Entities;

namespace TrackOrders.Services.Interfaces
{
    public interface IAuthService
    {

        Task<(User user, string token)> AuthenticateUser(string email, string password);

        Task<string> GenerateToken(User user);
    }
}
