using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTOs;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    // In-memory store for demo — replace with real user table/Identity in production
    private static readonly List<(string Email, string PasswordHash, string FirstName, string LastName)> _users = new();

    public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>Register a new user</summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        if (_users.Any(u => u.Email == dto.Email))
            return BadRequest(new { message = "Email already registered." });

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        _users.Add((dto.Email, hash, dto.FirstName, dto.LastName));
        _logger.LogInformation("New user registered: {Email}", dto.Email);
        return Ok(new { message = "Registration successful." });
    }

    /// <summary>Login and receive JWT token</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == default || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var fullName = $"{user.FirstName} {user.LastName}";
        var token = _tokenService.GenerateToken(dto.Email, fullName);
        _logger.LogInformation("User logged in: {Email}", dto.Email);

        return Ok(new AuthResponseDto(token, dto.Email, fullName, DateTime.UtcNow.AddHours(1)));
    }
}
