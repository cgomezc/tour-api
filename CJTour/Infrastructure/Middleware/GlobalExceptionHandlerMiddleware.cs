using System.Net;
using System.Text.Json;
using CJTour.Domain.Exceptions;
using CJTour.Contracts.DTOs;

using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorResponse = CreateErrorResponse(context, exception);
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.StatusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }

    private ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
    {
        var requestId = context.TraceIdentifier;
        var isDevelopment = _environment.IsDevelopment();

        // Log the exception
        _logger.LogError(exception, "An unhandled exception occurred. RequestId: {RequestId}", requestId);

        // Handle specific exception types
        switch (exception)
        {
            case ApiException apiException:
                return new ErrorResponse
                {
                    StatusCode = (int)apiException.StatusCode,
                    Message = apiException.Message,
                    Details = apiException.Details,
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };

            case DbUpdateException dbException:
                return new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Database operation failed",
                    Details = isDevelopment ? dbException.InnerException?.Message : null,
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };

            case UnauthorizedAccessException:
                return new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Access denied",
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };

            case ArgumentException argException:
                return new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = argException.Message,
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };

            case InvalidOperationException invalidOpException:
                return new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = invalidOpException.Message,
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };

            default:
                return new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = isDevelopment ? exception.Message : "An unexpected error occurred",
                    Details = isDevelopment ? exception.InnerException?.Message : null,
                    RequestId = requestId,
                    StackTrace = isDevelopment ? exception.StackTrace : null
                };
        }
    }
}

public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
} 