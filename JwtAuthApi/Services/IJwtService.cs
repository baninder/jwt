using JwtAuthApi.Models;

namespace JwtAuthApi.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        int? GetUserIdFromToken(string token);
        string? GetUserRoleFromToken(string token);
        DateTime GetTokenExpiration(string token);
    }
}
