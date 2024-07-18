using Microsoft.EntityFrameworkCore;
using TrackOrders.Data.Entities;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace TrackOrders.Data.Context
{
    public class TrackOrdersContext : DbContext
    {

        public DbSet<User> Users { get; init; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }

        public TrackOrdersContext()
        {
            
        }

        public TrackOrdersContext(DbContextOptions options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToCollection("users");
        }
    }
}
