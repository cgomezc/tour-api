using CJTour.Extensions;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Configuración básica de OpenTelemetry con versiones estables
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName: "CJTour", serviceVersion: "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();  // Para ver en logs del servicio
            
        // Jaeger exporter (opcional - solo si Jaeger está disponible)
        if (builder.Configuration.GetValue<bool>("OpenTelemetry:EnableJaeger", false))
        {
            tracerProviderBuilder.AddJaegerExporter(options =>
            {
                options.AgentHost = builder.Configuration.GetValue<string>("OpenTelemetry:JaegerHost", "localhost");
                options.AgentPort = builder.Configuration.GetValue<int>("OpenTelemetry:JaegerPort", 14268);
            });
        }
    });
builder.Services.AddOutputCache();
// Configure application builder with all services
builder.ConfigureApplicationBuilder();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Configure application pipeline and endpoints
app.ConfigureApplication();
app.UseOutputCache();
app.Run();
