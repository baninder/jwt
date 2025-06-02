using JwtAuthApi.Models;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthApi.Services
{
    public class UserService : IUserService
    {
        // In-memory storage for demo purposes - in production, use a real database
        private readonly List<User> _users = new()
        {
            new User
            {
                Id = 1,
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",                Password = "admin123", // In production, this would be hashed
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 2,
                Email = "user@example.com",
                FirstName = "Regular",
                LastName = "User",
                Password = "user123", // In production, this would be hashed
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(RegisterRequest request)
        {
            await Task.Delay(1); // Simulate async operation
            
            var user = new User
            {
                Id = _users.Count + 1,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password, // In production, this would be hashed
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _users.Add(user);
            return user;
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            await Task.Delay(1); // Simulate async operation
            // In production, verify hashed password
            return user.Password == password;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            await Task.Delay(1); // Simulate async operation
            return _users.Where(u => u.IsActive).ToList();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "SaltValue"));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}
