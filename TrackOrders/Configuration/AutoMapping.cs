using AutoMapper;
using TrackOrders.Data.Entities;
using TrackOrders.Data.ValueObjects;
using TrackOrders.ViewModels;

namespace TrackOrders.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<OrderRequest, Order>()
                .ForMember(o => o.CreatedDate , ac => ac.MapFrom(src => DateTime.Now))
                .ForMember(o => o.HasDelivered, ac => ac.MapFrom(src => false));

            CreateMap<Order, OrderResponse>();

            CreateMap<UserRequest,User>();

            CreateMap<User,UserResponse>();

            CreateMap<AddressRequest, Address>();
        }
    }
}
