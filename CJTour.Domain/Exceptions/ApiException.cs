using System.Net;

namespace CJTour.Domain.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? Details { get; }

    public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? details = null)
        : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }
}

public class NotFoundException : ApiException
{
    public NotFoundException(string message = "Resource not found", string? details = null)
        : base(message, HttpStatusCode.NotFound, details)
    {
    }
}

public class BadRequestException : ApiException
{
    public BadRequestException(string message = "Bad request", string? details = null)
        : base(message, HttpStatusCode.BadRequest, details)
    {
    }
}

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message = "Unauthorized", string? details = null)
        : base(message, HttpStatusCode.Unauthorized, details)
    {
    }
}

public class ForbiddenException : ApiException
{
    public ForbiddenException(string message = "Forbidden", string? details = null)
        : base(message, HttpStatusCode.Forbidden, details)
    {
    }
}

public class ValidationException : ApiException
{
    public ValidationException(string message = "Validation failed", string? details = null)
        : base(message, HttpStatusCode.BadRequest, details)
    {
    }
}