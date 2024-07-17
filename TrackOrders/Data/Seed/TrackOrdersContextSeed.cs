using TrackOrders.Data.Context;
using TrackOrders.Services.Interfaces;

namespace TrackOrders.Data.Seed
{
    public static class TrackOrdersContextSeed
    {

        public static void Seed(this TrackOrdersContext context, IPasswordHashService passwordHashService)
        {
            UserSeed.Seed(context, passwordHashService);
        }
    }
}
