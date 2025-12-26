# Error Handling Implementation

This document describes the comprehensive error handling implementation for the CJTour API, covering both development and production environments.

## Overview

The error handling system provides:
- **Global exception handling** with environment-specific responses
- **Custom exception types** for different error scenarios
- **Consistent error response format** across all endpoints
- **Validation error handling** with detailed field-level errors
- **Comprehensive logging** for debugging and monitoring
- **Health check endpoints** for monitoring

## Architecture

### 1. Global Exception Handler Middleware
- **File**: `Infrastructure/Middleware/GlobalExceptionHandlerMiddleware.cs`
- **Purpose**: Catches all unhandled exceptions and returns consistent error responses
- **Features**:
  - Environment-specific error details (stack traces only in development)
  - Request ID tracking for debugging
  - Structured logging of all errors
  - Custom exception type handling

### 2. Custom Exception Types
- **File**: `Core/Exceptions/ApiException.cs`
- **Types**:
  - `ApiException`: Base exception with HTTP status code
  - `NotFoundException`: 404 errors
  - `BadRequestException`: 400 errors
  - `UnauthorizedException`: 401 errors
  - `ForbiddenException`: 403 errors
  - `ValidationException`: 400 validation errors

### 3. Error Response DTOs
- **File**: `Core/DTOs/ErrorResponse.cs`
- **Types**:
  - `ErrorResponse`: Standard error response
  - `ValidationErrorResponse`: Validation-specific errors with field details

### 4. Validation Filter
- **File**: `Infrastructure/Filters/ValidationFilter.cs`
- **Purpose**: Intercepts model validation errors and returns structured responses
- **Features**:
  - Field-level validation error details
  - Consistent error format
  - Request ID tracking

## Usage Examples

### Throwing Custom Exceptions

```csharp
// In your service or controller
public async Task<Tour> GetTourByIdAsync(int id)
{
    var tour = await _tourRepository.GetByIdAsync(id);
    
    if (tour == null)
    {
        throw new NotFoundException($"Tour with ID {id} was not found");
    }
    
    return tour;
}
```

### Model Validation
The validation filter automatically handles model validation errors:

```csharp
[HttpPost]
public IActionResult CreateTour([FromBody] TourDto tourDto)
{
    // If tourDto fails validation, the filter will return a ValidationErrorResponse
    // with detailed field-level errors
    return Ok(tour);
}
```

### Testing Error Handling
Use the `ErrorTestController` to test different error scenarios:

```bash
# Test different error types
GET /api/ErrorTest/not-found
GET /api/ErrorTest/bad-request
GET /api/ErrorTest/unauthorized
GET /api/ErrorTest/validation
GET /api/ErrorTest/database
GET /api/ErrorTest/unexpected

# Test model validation
POST /api/ErrorTest/validation-test
Content-Type: application/json

{
  "name": "",
  "email": "invalid-email",
  "age": 150
}
```

## Error Response Format

### Standard Error Response
```json
{
  "statusCode": 404,
  "message": "Tour with ID 123 was not found",
  "details": null,
  "timestamp": "2024-01-15T10:30:00.000Z",
  "requestId": "0HMQ1234567890",
  "traceId": null,
  "validationErrors": null,
  "stackTrace": null
}
```

### Validation Error Response
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "details": null,
  "timestamp": "2024-01-15T10:30:00.000Z",
  "requestId": "0HMQ1234567890",
  "traceId": null,
  "validationErrors": {
    "name": ["The Name field is required."],
    "email": ["The Email field is not a valid e-mail address."],
    "age": ["The field Age must be between 1 and 120."]
  },
  "stackTrace": null
}
```

## Environment-Specific Behavior

### Development Environment
- **Detailed error messages** with exception details
- **Stack traces** included in responses
- **Verbose logging** for debugging
- **Swagger UI** enabled for API testing

### Production Environment
- **Generic error messages** for security
- **No stack traces** in responses
- **Structured logging** for monitoring
- **HSTS** enabled for security
- **Minimal error details** to prevent information leakage

## Configuration

### Development Settings (`appsettings.Development.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "CJTour": "Debug"
    }
  },
  "ApiSettings": {
    "EnableDetailedErrors": true
  }
}
```

### Production Settings (`appsettings.Production.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "CJTour": "Information"
    }
  },
  "ApiSettings": {
    "EnableDetailedErrors": false
  }
}
```

## Health Check Endpoints

### Basic Health Check
```bash
GET /health
```

Response:
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00.000Z",
  "environment": "Development",
  "version": "1.0.0"
}
```

### API Status
```bash
GET /api/status
```

Response:
```json
{
  "status": "API is running",
  "timestamp": "2024-01-15T10:30:00.000Z",
  "environment": "Development"
}
```

## Best Practices

### 1. Use Custom Exceptions
Always use the appropriate custom exception type:
```csharp
// Good
throw new NotFoundException("User not found");

// Avoid
throw new Exception("User not found");
```

### 2. Log Appropriately
```csharp
_logger.LogInformation("User {UserId} accessed tour {TourId}", userId, tourId);
_logger.LogWarning("Rate limit exceeded for user {UserId}", userId);
_logger.LogError(exception, "Failed to process payment for user {UserId}", userId);
```

### 3. Handle Database Errors
```csharp
try
{
    await _context.SaveChangesAsync();
}
catch (DbUpdateException ex)
{
    _logger.LogError(ex, "Database update failed");
    throw new BadRequestException("Unable to save changes");
}
```

### 4. Validate Input
Always validate input using data annotations or FluentValidation:
```csharp
public class TourDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 1000)]
    public decimal Price { get; set; }
}
```

## Monitoring and Debugging

### Request Tracking
Every error response includes a `requestId` that can be used to trace the request through logs.

### Logging Levels
- **Debug**: Detailed information for debugging
- **Information**: General application flow
- **Warning**: Unexpected situations that don't stop execution
- **Error**: Errors that need immediate attention

### Structured Logging
All logs use structured logging with relevant context:
```csharp
_logger.LogError(exception, "Failed to create tour. UserId: {UserId}, TourName: {TourName}", userId, tourName);
```

## Troubleshooting

### Common Issues

1. **Validation errors not showing field details**
   - Ensure `ValidationFilter` is registered in `Program.cs`
   - Check that model has proper data annotations

2. **Stack traces not showing in development**
   - Verify `ASPNETCORE_ENVIRONMENT=Development`
   - Check `EnableDetailedErrors` setting

3. **Custom exceptions not handled properly**
   - Ensure `GlobalExceptionHandlerMiddleware` is registered before other middleware
   - Verify exception inherits from `ApiException`

### Debugging Tips

1. **Use the ErrorTestController** to test different error scenarios
2. **Check logs** for detailed error information
3. **Use request IDs** to trace errors through the system
4. **Test in both environments** to ensure proper behavior 