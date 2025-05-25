using CloudinaryDotNet;
using Entities;
using Entities.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Repository.Contracts;
using Service.Contracts;
using Services;
using Services.Extensions;
using System.Text;

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
            services.AddScoped<IAddressRepository, AddressRepository>();

            return services;
        }

        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped(typeof(IAddressService<>), typeof(AddressService<>));

            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
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

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = Environment.GetEnvironmentVariable("Jwt_Secret");
            var issuer = Environment.GetEnvironmentVariable("Jwt_Issuer");
            var audience = Environment.GetEnvironmentVariable("Jwt_Audience");

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters   //tham so xac thuc cho jwt
                {
                    //cap token: true-> dich vu, false->tu cap
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ClockSkew = TimeSpan.Zero, // bo tg chenh lech
                    ValidateLifetime = true,    //xac thuc thoi gian ton tai cua token

                    //ky vao token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public static void ConfigurePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("All", policy =>
                    policy.RequireRole("Admin", "Staff", "Customer"));
                options.AddPolicy("CustomerOnly", policy =>
                    policy.RequireRole("Customer"));
                options.AddPolicy("StaffOnly", policy =>
                    policy.RequireRole("Staff"));
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

            });
        }
    }
}
