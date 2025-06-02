using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Models;

namespace JwtAuthApi.Core.Repositories
{
    /// <summary>
    /// In-memory implementation of generic repository pattern
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class
        where TKey : notnull
    {
        protected readonly Dictionary<TKey, TEntity> _entities;
        protected readonly Func<TEntity, TKey> _keySelector;

        public InMemoryRepository(Func<TEntity, TKey> keySelector)
        {
            _entities = new Dictionary<TKey, TEntity>();
            _keySelector = keySelector;
        }

        public virtual Task<TEntity?> GetByIdAsync(TKey id)
        {
            _entities.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_entities.Values.AsEnumerable());
        }

        public virtual Task<TEntity> AddAsync(TEntity entity)
        {
            var key = _keySelector(entity);
            _entities[key] = entity;
            return Task.FromResult(entity);
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            var key = _keySelector(entity);
            if (!_entities.ContainsKey(key))
                throw new InvalidOperationException($"Entity with key {key} not found");
            
            _entities[key] = entity;
            return Task.FromResult(entity);
        }

        public virtual Task<bool> DeleteAsync(TKey id)
        {
            return Task.FromResult(_entities.Remove(id));
        }

        public virtual Task<bool> ExistsAsync(TKey id)
        {
            return Task.FromResult(_entities.ContainsKey(id));
        }
    }
}
