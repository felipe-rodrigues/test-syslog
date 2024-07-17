using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Driver;
using TrackOrders.Data.Context;

namespace TrackOrders.Data
{
    public static class DIExtensions
    {

        public static void AddDatabase(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<TrackOrdersContext>(op => op.UseMongoDB(connectionString, "trackorders"));
        }
    }
}
