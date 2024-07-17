using AutoMapper;

namespace TrackOrders.Configuration
{
    public static class DIExtensions
    {

        public static void AddMapper(this IServiceCollection services) 
        {
            var mappingConfig = new MapperConfiguration(c =>
            {
                c.AddProfile(new AutoMapping());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
