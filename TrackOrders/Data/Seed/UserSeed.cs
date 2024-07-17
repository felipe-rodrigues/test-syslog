
using TrackOrders.Data.Context;
using TrackOrders.Services.Interfaces;

namespace TrackOrders.Data.Seed
{
    public static class UserSeed
    {


        public static void Seed(TrackOrdersContext context, IPasswordHashService passwordHashService)
        {
            if (context.Users.Count() == 0)
            {
                context.Users.Add(new Entities.User()
                { 
                    Email = "admin@gmail.com",
                    Password = passwordHashService.Hash("12345"),
                    Name = "Admin"
                });

                context.SaveChanges();
            }
        }
    }
}
