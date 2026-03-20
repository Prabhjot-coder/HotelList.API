namespace WebApplication4.DTOs;

public record RegisterDto(string FirstName, string LastName, string Email, string Password);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, string Email, string FullName, DateTime ExpiresAt);
