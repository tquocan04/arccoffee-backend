using ArcCoffee_backend.Extensions;
using DotNetEnv;
using Mapster;
using Microsoft.OpenApi.Models;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.ConfigureDatabase();

builder.Services.AddMapster();
MappingConfig.Configure();

builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.ConfigureCloudinary();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    // Thêm cấu hình JWT Bearer vào Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "Chèn JWT token vào đây"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});

var app = builder.Build();

app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
