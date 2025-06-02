using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Core.Repositories;

namespace JwtAuthApi.Core.UnitOfWork
{
    /// <summary>
    /// In-memory implementation of Unit of Work pattern
    /// </summary>
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IUserRepository> _userRepository;
        private bool _disposed = false;

        public InMemoryUnitOfWork()
        {
            _userRepository = new Lazy<IUserRepository>(() => new InMemoryUserRepository());
        }

        public IUserRepository Users => _userRepository.Value;

        public Task<int> SaveChangesAsync()
        {
            // In a real implementation with EF Core, this would save all changes
            // For in-memory, we don't need to do anything as changes are immediate
            return Task.FromResult(1);
        }

        public Task BeginTransactionAsync()
        {
            // In-memory implementation doesn't support transactions
            // In real EF Core implementation, this would begin a database transaction
            return Task.CompletedTask;
        }

        public Task CommitTransactionAsync()
        {
            // In-memory implementation doesn't support transactions
            return Task.CompletedTask;
        }

        public Task RollbackTransactionAsync()
        {
            // In-memory implementation doesn't support transactions
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // Dispose managed resources
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
