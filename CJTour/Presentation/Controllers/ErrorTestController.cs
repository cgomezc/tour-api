using Microsoft.AspNetCore.Mvc;
using CJTour.Domain.Exceptions;
using CJTour.Contracts.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CJTour.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorTestController : ControllerBase
{
    private readonly ILogger<ErrorTestController> _logger;

    public ErrorTestController(ILogger<ErrorTestController> logger)
    {
        _logger = logger;
    }

    [HttpGet("not-found")]
    public IActionResult TestNotFound()
    {
        throw new NotFoundException("The requested resource was not found");
    }

    [HttpGet("bad-request")]
    public IActionResult TestBadRequest()
    {
        throw new BadRequestException("Invalid request parameters");
    }

    [HttpGet("unauthorized")]
    public IActionResult TestUnauthorized()
    {
        throw new UnauthorizedException("Authentication required");
    }

    [HttpGet("forbidden")]
    public IActionResult TestForbidden()
    {
        throw new ForbiddenException("Access denied");
    }

    [HttpGet("validation")]
    public IActionResult TestValidation()
    {
        throw new Domain.Exceptions.ValidationException("Validation failed", "Field 'email' is required");
    }

    [HttpGet("database")]
    public IActionResult TestDatabaseError()
    {
        throw new InvalidOperationException("Database connection failed");
    }

    [HttpGet("unexpected")]
    public IActionResult TestUnexpectedError()
    {
        throw new Exception("An unexpected error occurred");
    }

    [HttpPost("validation-test")]
    public IActionResult TestModelValidation([FromBody] TestModel model)
    {
        // This will trigger validation errors if the model is invalid
        return Ok(new { message = "Validation passed", data = model });
    }
}

public class TestModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(1, 120)]
    public int Age { get; set; }
} 