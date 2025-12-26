using CJTour.Application.Interfaces;
using CJTour.Contracts.DTOs;
using CJTour.Domain.Entities;
using CJTour.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CJTour.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;
    private readonly int _refreshTokenExpirationDays;

    public AuthService(
        IUserRepository userRepository, 
        IJwtTokenService jwtTokenService, 
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
        
        var jwtSettings = _configuration.GetSection("JwtSettings");
        _refreshTokenExpirationDays = int.TryParse(jwtSettings["RefreshTokenExpirationInDays"], out int days) ? days : 7;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var userDto = MapToUserDto(user);
        var accessToken = _jwtTokenService.GenerateAccessToken(userDto);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // In a real application, you should store the refresh token in the database
        // For now, we'll just return it

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var expirationMinutes = int.TryParse(jwtSettings["ExpirationInMinutes"], out int minutes) ? minutes : 60;

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            User = userDto
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
    {
        if (await _userRepository.ExistsAsync(registerRequest.Email))
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Email = registerRequest.Email,
            Password = HashPassword(registerRequest.Password),
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            PhoneNumber = registerRequest.PhoneNumber,
            DateOfBirth = registerRequest.DateOfBirth,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.CreateAsync(user);
        var userDto = MapToUserDto(createdUser);
        var accessToken = _jwtTokenService.GenerateAccessToken(userDto);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var expirationMinutes = int.TryParse(jwtSettings["ExpirationInMinutes"], out int minutes) ? minutes : 60;

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            User = userDto
        };
    }

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
    {
        // In a real application, you should validate the refresh token against stored tokens
        // For now, we'll implement a basic version
        throw new NotImplementedException("Refresh token functionality requires database storage implementation");
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null ? MapToUserDto(user) : null;
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(email);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == hashedPassword;
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            CreatedAt = user.CreatedAt
        };
    }
}