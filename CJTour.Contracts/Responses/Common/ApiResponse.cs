namespace CJTour.Contracts.Responses.Common;

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null,
    IEnumerable<string>? Errors = null
);

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
        => new(true, data, message);

    public static ApiResponse<T> Failure<T>(string message, IEnumerable<string>? errors = null)
        => new(false, default, message, errors);
        
    public static ApiResponse<T> NotFound<T>(string message = "Resource not found")
        => new(false, default, message);
        
    public static ApiResponse<T> ValidationError<T>(IEnumerable<string> errors)
        => new(false, default, "Validation failed", errors);
}