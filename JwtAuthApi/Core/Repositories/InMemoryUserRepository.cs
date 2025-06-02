using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Models;

namespace JwtAuthApi.Core.Repositories
{
    /// <summary>
    /// In-memory implementation of User repository with business-specific operations
    /// </summary>
    public class InMemoryUserRepository : InMemoryRepository<User, int>, IUserRepository
    {
        public InMemoryUserRepository() : base(user => user.Id)
        {
            SeedData();
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _entities.Values.FirstOrDefault(u => 
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            var exists = _entities.Values.Any(u => 
                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            var users = _entities.Values.Where(u => 
                string.Equals(u.Role, role, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(users);
        }

        public Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            var activeUsers = _entities.Values.Where(u => u.IsActive);
            return Task.FromResult(activeUsers);
        }

        private void SeedData()
        {
            var users = new[]
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "password123", // In real app, this would be hashed
                    Role = "User",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Password = "admin123", // In real app, this would be hashed
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new User
                {
                    Id = 3,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    Password = "user123", // In real app, this would be hashed
                    Role = "User",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-60)
                }
            };

            foreach (var user in users)
            {
                _entities[user.Id] = user;
            }
        }
    }
}
