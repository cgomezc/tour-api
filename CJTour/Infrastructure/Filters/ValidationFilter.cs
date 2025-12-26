using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CJTour.Contracts.DTOs;
using System.Text.Json;

namespace CJTour.Infrastructure.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var validationErrors = new Dictionary<string, string[]>();
            
            foreach (var modelState in context.ModelState)
            {
                if (modelState.Value.Errors.Count > 0)
                {
                    validationErrors[modelState.Key] = modelState.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray();
                }
            }

            var errorResponse = new ValidationErrorResponse(validationErrors)
            {
                RequestId = context.HttpContext.TraceIdentifier
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
            
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution
    }
}

public static class ValidationFilterExtensions
{
    public static IServiceCollection AddValidationFilter(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        return services;
    }
} 