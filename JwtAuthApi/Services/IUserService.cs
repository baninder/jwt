using JwtAuthApi.Models;

namespace JwtAuthApi.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(RegisterRequest request);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UserExistsAsync(string email);
    }
}
