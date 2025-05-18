using Repositories;
using Repository.Contracts;
using Service.Contracts;
using Services;

namespace ArcCoffee_backend.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();

            return services;
        }
        
        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IShippingService, ShippingService>();

            return services;
        }
    }
}
