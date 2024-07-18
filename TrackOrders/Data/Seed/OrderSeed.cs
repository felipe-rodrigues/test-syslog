using TrackOrders.Data.Context;
using TrackOrders.Services.Interfaces;

namespace TrackOrders.Data.Seed
{
    public static class OrderSeed
    {

        public static void Seed(TrackOrdersContext context)
        {
            if (context.Orders.Count() == 0)
            {

                context.Orders.Add(new Entities.Order()
                {
                    Number = "000",
                    CreatedDate = DateTime.Now,
                    Description = "descrption",
                    HasDelivered = false,
                    Value = 10,
                    DeliveryAddress = new ValueObjects.Address()
                    {
                        Number = "1004",
                        City = "São Paulo",
                        Code = "01310100",
                        District = "Bela Vista",
                        State = "SP",
                        Street = "Avenida Paulista"
                    }

                });

                context.SaveChanges();
            }
        }
    }
}
