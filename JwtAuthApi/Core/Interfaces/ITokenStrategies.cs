using System.Security.Claims;

namespace JwtAuthApi.Core.Interfaces
{
    /// <summary>
    /// Defines contract for token generation strategies
    /// </summary>
    /// <typeparam name="TUser">User entity type</typeparam>
    /// <typeparam name="TTokenResult">Token generation result type</typeparam>
    public interface ITokenGenerationStrategy<in TUser, out TTokenResult>
    {
        TTokenResult GenerateToken(TUser user);
    }

    /// <summary>
    /// Defines contract for token validation strategies
    /// </summary>
    public interface ITokenValidationStrategy
    {
        bool ValidateToken(string token);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        DateTime GetTokenExpiration(string token);
    }

    /// <summary>
    /// Defines contract for token extraction strategies
    /// </summary>
    public interface ITokenExtractionStrategy
    {
        string? ExtractToken(string authorizationHeader);
    }
}
