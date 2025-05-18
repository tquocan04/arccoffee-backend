using Repositories;
using Repository.Contracts;

namespace ArcCoffee_backend.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
