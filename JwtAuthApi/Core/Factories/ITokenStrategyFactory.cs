using JwtAuthApi.Core.Interfaces;

namespace JwtAuthApi.Core.Factories
{
    /// <summary>
    /// Factory interface for creating token strategies
    /// </summary>
    public interface ITokenStrategyFactory
    {
        ITokenGenerationStrategy<TUser, TResult> CreateGenerationStrategy<TUser, TResult>();
        ITokenValidationStrategy CreateValidationStrategy();
        ITokenExtractionStrategy CreateExtractionStrategy();
    }
}
