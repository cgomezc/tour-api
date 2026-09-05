using CJTour.Application.Interfaces;
using CJTour.Application.Services;
using CJTour.Domain.Interfaces;
using CJTour.Infrastructure.Persistence.Context;
using CJTour.Infrastructure.Persistence.Repositories;
using CJTour.Infrastructure.Filters;
using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<ITourService, TourService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IDestinationService, DestinationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IBenchmarkService, BenchmarkService>();
        services.AddSingleton<ICacheService, CacheService>();
        
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register database context
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<ITourPackageRepository, TourPackageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITourCategoryRepository, TourCategoryRepository>();
        services.AddScoped<IDestinationRepository, DestinationRepository>();

        // Register validation filter
        services.AddValidationFilter();

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("LocalDevCors", policy =>
            {
                policy.WithOrigins(
                        "https://localhost:3000",
                        "http://localhost:3000",
                        "https://localhost:7053",
                        "http://localhost:5173",
                        "https://localhost:5173",
                        "http://localhost:5174",
                        "https://localhost:5174"
                      )
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("UserPolicy", policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}