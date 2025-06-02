using JwtAuthApi.Core.Interfaces;

namespace JwtAuthApi.Core.Strategies
{
    /// <summary>
    /// Bearer token extraction strategy implementation
    /// </summary>
    public class BearerTokenExtractionStrategy : ITokenExtractionStrategy
    {
        private const string BearerPrefix = "Bearer ";

        public string? ExtractToken(string authorizationHeader)
        {
            if (string.IsNullOrWhiteSpace(authorizationHeader))
                return null;

            if (!authorizationHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
                return null;

            return authorizationHeader.Substring(BearerPrefix.Length).Trim();
        }
    }
}
