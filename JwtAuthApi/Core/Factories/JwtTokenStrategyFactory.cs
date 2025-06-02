using JwtAuthApi.Configuration;
using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Core.Strategies;
using JwtAuthApi.Models;
using Microsoft.Extensions.Options;

namespace JwtAuthApi.Core.Factories
{
    /// <summary>
    /// Factory implementation for creating JWT token strategies
    /// </summary>
    public class JwtTokenStrategyFactory : ITokenStrategyFactory
    {
        private readonly IOptions<JwtSettings> _jwtSettings;

        public JwtTokenStrategyFactory(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public ITokenGenerationStrategy<TUser, TResult> CreateGenerationStrategy<TUser, TResult>()
        {
            // For now, we only support User -> string token generation
            // This can be extended in the future for different user types and token formats
            if (typeof(TUser) == typeof(User) && typeof(TResult) == typeof(string))
            {
                return (ITokenGenerationStrategy<TUser, TResult>)(object)new JwtTokenGenerationStrategy(_jwtSettings);
            }

            throw new NotSupportedException($"Token generation strategy for {typeof(TUser).Name} -> {typeof(TResult).Name} is not supported");
        }

        public ITokenValidationStrategy CreateValidationStrategy()
        {
            return new JwtTokenValidationStrategy(_jwtSettings);
        }

        public ITokenExtractionStrategy CreateExtractionStrategy()
        {
            return new BearerTokenExtractionStrategy();
        }
    }
}
