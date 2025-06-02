using JwtAuthApi.Core.Interfaces;

namespace JwtAuthApi.Core.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface for managing multiple repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
