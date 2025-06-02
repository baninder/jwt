using System.Security.Claims;
using System.Security.Cryptography;
using JwtAuthApi.Core.Factories;
using JwtAuthApi.Core.Logging;
using JwtAuthApi.Models;
using JwtAuthApi.Services;

namespace JwtAuthApi.Core.Services
{
    /// <summary>
    /// Enterprise-level JWT service using Strategy and Factory patterns
    /// </summary>
    public class EnterpriseJwtService : IJwtService
    {
        private readonly ITokenStrategyFactory _tokenStrategyFactory;
        private readonly IAppLogger<EnterpriseJwtService> _logger;

        public EnterpriseJwtService(
            ITokenStrategyFactory tokenStrategyFactory,
            IAppLogger<EnterpriseJwtService> logger)
        {
            _tokenStrategyFactory = tokenStrategyFactory;
            _logger = logger;
        }

        public string GenerateJwtToken(User user)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user {UserId}", user.Id);
                
                var generationStrategy = _tokenStrategyFactory.CreateGenerationStrategy<User, string>();
                var token = generationStrategy.GenerateToken(user);
                
                _logger.LogInformation("JWT token generated successfully for user {UserId}", user.Id);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token for user {UserId}", user.Id);
                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                _logger.LogDebug("Generating refresh token");
                
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                
                _logger.LogDebug("Refresh token generated successfully");
                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate refresh token");
                throw;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                _logger.LogDebug("Validating JWT token");
                
                var validationStrategy = _tokenStrategyFactory.CreateValidationStrategy();
                var isValid = validationStrategy.ValidateToken(token);
                  _logger.LogDebug("Token validation result: {IsValid}", isValid);
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Token validation failed: {Error}", ex.Message);
                return false;
            }
        }

        public int? GetUserIdFromToken(string token)
        {
            try
            {
                _logger.LogDebug("Extracting user ID from token");
                
                var validationStrategy = _tokenStrategyFactory.CreateValidationStrategy();
                var principal = validationStrategy.GetPrincipalFromToken(token);
                
                if (principal == null)
                {
                    _logger.LogWarning("Failed to get principal from token");
                    return null;
                }

                var userIdClaim = principal.FindFirst("user_id") ?? principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    _logger.LogDebug("Successfully extracted user ID {UserId} from token", userId);
                    return userId;
                }                _logger.LogWarning("User ID claim not found or invalid in token");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to extract user ID from token: {Error}", ex.Message);
                return null;
            }
        }

        public string? GetUserRoleFromToken(string token)
        {
            try
            {
                _logger.LogDebug("Extracting user role from token");
                
                var validationStrategy = _tokenStrategyFactory.CreateValidationStrategy();
                var principal = validationStrategy.GetPrincipalFromToken(token);
                
                if (principal == null)
                {
                    _logger.LogWarning("Failed to get principal from token");
                    return null;
                }

                var roleClaim = principal.FindFirst(ClaimTypes.Role);
                var role = roleClaim?.Value;
                  _logger.LogDebug("Successfully extracted role {Role} from token", role ?? "null");
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to extract user role from token: {Error}", ex.Message);
                return null;
            }
        }

        public DateTime GetTokenExpiration(string token)
        {
            try
            {
                _logger.LogDebug("Getting token expiration");
                
                var validationStrategy = _tokenStrategyFactory.CreateValidationStrategy();
                var expiration = validationStrategy.GetTokenExpiration(token);
                  _logger.LogDebug("Token expiration: {Expiration}", expiration);
                return expiration;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to get token expiration: {Error}", ex.Message);
                return DateTime.MinValue;
            }
        }
    }
}
