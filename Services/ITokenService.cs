using WebApplication4.DTOs;
namespace WebApplication4.Services;

public interface ITokenService
{
    string GenerateToken(string email, string fullName);
}
