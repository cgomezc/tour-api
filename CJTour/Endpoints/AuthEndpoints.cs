using CJTour.Application.Interfaces;
using CJTour.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CJTour.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var authApi = app.MapGroup("/api/auth").WithTags("Authentication");

        // POST /api/auth/login
        authApi.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("User login")
            .WithDescription("Authenticates a user and returns a JWT token")
            .Produces<AuthResponseDto>(200)
            .Produces(400)
            .Produces(401)
            .AllowAnonymous();

        // POST /api/auth/register
        authApi.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("User registration")
            .WithDescription("Registers a new user and returns a JWT token")
            .Produces<AuthResponseDto>(201)
            .Produces(400)
            .Produces(409)
            .AllowAnonymous();

        // POST /api/auth/refresh
        authApi.MapPost("/refresh", RefreshTokenAsync)
            .WithName("RefreshToken")
            .WithSummary("Refresh JWT token")
            .WithDescription("Refreshes an expired JWT token using a refresh token")
            .Produces<AuthResponseDto>(200)
            .Produces(400)
            .Produces(401)
            .AllowAnonymous();

        // GET /api/auth/me
        authApi.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .WithSummary("Get current user")
            .WithDescription("Gets the current authenticated user's information")
            .Produces<UserDto>(200)
            .Produces(401)
            .RequireAuthorization();

        // GET /api/auth/validate
        authApi.MapGet("/validate", ValidateTokenAsync)
            .WithName("ValidateToken")
            .WithSummary("Validate JWT token")
            .WithDescription("Validates if the current JWT token is valid")
            .Produces<bool>(200)
            .Produces(401)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequestDto loginRequest,
        IAuthService authService)
    {
        try
        {
            var response = await authService.LoginAsync(loginRequest);
            return Results.Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred during login",
                detail: ex.Message
            );
        }
    }

    private static async Task<IResult> RegisterAsync(
        RegisterRequestDto registerRequest,
        IAuthService authService)
    {
        try
        {
            var response = await authService.RegisterAsync(registerRequest);
            return Results.Created($"/api/auth/users/{response.User.UserId}", response);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred during registration",
                detail: ex.Message
            );
        }
    }

    private static async Task<IResult> RefreshTokenAsync(
        RefreshTokenRequestDto refreshTokenRequest,
        IAuthService authService)
    {
        try
        {
            var response = await authService.RefreshTokenAsync(refreshTokenRequest);
            return Results.Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
        catch (NotImplementedException ex)
        {
            return Results.Problem(
                statusCode: 501,
                title: "Refresh token not implemented",
                detail: ex.Message
            );
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred during token refresh",
                detail: ex.Message
            );
        }
    }

    private static async Task<IResult> GetCurrentUserAsync(
        HttpContext context,
        IAuthService authService)
    {
        try
        {
            var email = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Results.Unauthorized();
            }

            var user = await authService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Results.NotFound(new { message = "User not found" });
            }

            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                statusCode: 500,
                title: "An error occurred while getting user information",
                detail: ex.Message
            );
        }
    }

    private static Task<IResult> ValidateTokenAsync()
    {
        // If we reach here, the token is valid (thanks to RequireAuthorization)
        return Task.FromResult(Results.Ok(true));
    }
}