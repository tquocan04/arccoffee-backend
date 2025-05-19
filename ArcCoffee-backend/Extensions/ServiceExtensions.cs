using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repository.Contracts;
using Service.Contracts;
using Services;
using Services.Extensions;

namespace ArcCoffee_backend.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing.");
            }

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("ArcCoffee-backend")));

            return services;
        }

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

        public static IServiceCollection ConfigureCloudinary(this IServiceCollection services)
        {
            services.Configure<CloudinarySettings>(options =>
            {
                options.CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
                options.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
                options.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");
            });

            return services;
        }
    }
}
