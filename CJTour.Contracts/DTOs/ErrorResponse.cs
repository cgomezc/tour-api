


using System.Text.Json.Serialization;

namespace CJTour.Contracts.DTOs;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string? TraceId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string[]>? ValidationErrors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTrace { get; set; }

    public ErrorResponse()
    {
        Timestamp = DateTime.UtcNow;
    }

    public ErrorResponse(string message, int statusCode = 500)
    {
        Message = message;
        StatusCode = statusCode;
        Timestamp = DateTime.UtcNow;
    }
}

public class ValidationErrorResponse : ErrorResponse
{
    public ValidationErrorResponse() : base("Validation failed", 400)
    {
        ValidationErrors = new Dictionary<string, string[]>();
    }

    public ValidationErrorResponse(Dictionary<string, string[]> validationErrors) : base("Validation failed", 400)
    {
        ValidationErrors = validationErrors;
    }
}