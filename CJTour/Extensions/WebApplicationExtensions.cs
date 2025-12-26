using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using CJTour.Contracts.DTOs;
using CJTour.Infrastructure.Middleware;
using CJTour.Endpoints;

namespace CJTour.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Configure global exception handling
        app.UseGlobalExceptionHandler();

        // Configure the HTTP request pipeline
        ConfigurePipeline(app);

        // Configure routing
        ConfigureRouting(app);

        // Configure endpoints
        ConfigureEndpoints(app);

        return app;
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CJTour API v1");
                c.RoutePrefix = "swagger";
            });
            
            // Development-specific error handling
            app.UseExceptionHandler(ConfigureDevelopmentErrorHandling);
        }
        else
        {
            // Production error handling
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            
            // Production-specific error endpoint
            app.Map("/Error", () => Results.Problem(
                title: "An error occurred",
                statusCode: 500,
                detail: "Please try again later or contact support if the problem persists."
            ));
        }

        app.UseHttpsRedirection();
        app.UseCors("LocalDevCors");

        // Habilitar Authentication y Authorization
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static void ConfigureRouting(WebApplication app)
    {
        // Map controllers (for remaining controllers like BenchmarkController)
        app.MapControllers();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        // Map tour endpoints
        app.MapTourEndpoints();

        // Map auth endpoints
        app.MapAuthEndpoints();

        // Health check endpoint
        app.MapGet("/health", () => new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = app.Environment.EnvironmentName,
            Version = "1.0.0"
        })
        .WithName("HealthCheck")
        .WithSummary("Health check endpoint")
        .WithDescription("Returns the health status of the API")
        .WithTags("Health");

        // API status endpoint
        app.MapGet("/api/status", () => new
        {
            Status = "API is running",
            Timestamp = DateTime.UtcNow,
            Environment = app.Environment.EnvironmentName
        })
        .WithName("ApiStatus")
        .WithSummary("API status endpoint")
        .WithDescription("Returns the status of the API")
        .WithTags("Status");
    }

    private static void ConfigureDevelopmentErrorHandling(IApplicationBuilder errorApp)
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var exception = context.Features.Get<IExceptionHandlerFeature>();
            
            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = exception?.Error?.Message ?? "An error occurred",
                Details = exception?.Error?.InnerException?.Message,
                RequestId = context.TraceIdentifier,
                StackTrace = exception?.Error?.StackTrace
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        });
    }
}