using CJTour.Contracts.DTOs;

namespace CJTour.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<bool> UserExistsAsync(string email);
}

public interface IJwtTokenService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    int? GetUserIdFromToken(string token);
}