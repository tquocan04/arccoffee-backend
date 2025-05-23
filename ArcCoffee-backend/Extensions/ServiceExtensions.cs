using CloudinaryDotNet;
using Entities;
using Entities.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories;
using Repository.Contracts;
using Service.Contracts;
using Services;
using Services.Extensions;
using System;

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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();

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

            services.AddSingleton<Cloudinary>(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
                return new Cloudinary(account);
            });

            services.AddScoped<CloudinaryService>();

            return services;
        }
    }
}
