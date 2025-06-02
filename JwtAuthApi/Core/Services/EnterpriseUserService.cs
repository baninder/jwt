using JwtAuthApi.Core.Common;
using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Core.Specifications.UserSpecifications;
using JwtAuthApi.Models;
using JwtAuthApi.Services;

namespace JwtAuthApi.Core.Services
{    /// <summary>
    /// Enterprise-level User service using Repository and Specification patterns
    /// </summary>
    public class EnterpriseUserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public EnterpriseUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Legacy interface methods for backward compatibility
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var result = await GetByEmailAsync(email);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var result = await GetByIdAsync(id);
            return result.IsSuccess ? result.Data : null;
        }

        public async Task<User> CreateUserAsync(RegisterRequest request)
        {
            var result = await RegisterAsync(request);
            if (result.IsSuccess)
                return result.Data!;
            
            throw new InvalidOperationException(result.ErrorMessage);
        }        public Task<bool> ValidatePasswordAsync(User user, string password)
        {
            // In real app, verify hashed password
            return Task.FromResult(user.Password == password);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var result = await GetActiveUsersAsync();
            return result.IsSuccess ? result.Data!.ToList() : new List<User>();
        }

        public Task<bool> UserExistsAsync(string email)
        {
            return _userRepository.EmailExistsAsync(email);
        }

        // New enterprise methods with Result pattern

        public async Task<Result<User>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Check if email already exists
                if (await _userRepository.EmailExistsAsync(request.Email))
                {
                    return Result<User>.Failure("Email already exists", "EMAIL_EXISTS");
                }

                // Get next available ID
                var allUsers = await _userRepository.GetAllAsync();
                var nextId = allUsers.Any() ? allUsers.Max(u => u.Id) + 1 : 1;

                var user = new User
                {
                    Id = nextId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password, // In real app, hash this
                    Role = "User", // Default role
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(user);
                return Result<User>.Success(createdUser);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Registration failed: {ex.Message}", "REGISTRATION_ERROR");
            }
        }

        public async Task<Result<User>> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                
                if (user == null)
                {
                    return Result<User>.Failure("Invalid credentials", "INVALID_CREDENTIALS");
                }

                if (!user.IsActive)
                {
                    return Result<User>.Failure("Account is inactive", "ACCOUNT_INACTIVE");
                }

                // In real app, verify hashed password
                if (user.Password != request.Password)
                {
                    return Result<User>.Failure("Invalid credentials", "INVALID_CREDENTIALS");
                }

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Login failed: {ex.Message}", "LOGIN_ERROR");
            }
        }

        public async Task<Result<User>> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user != null 
                    ? Result<User>.Success(user)
                    : Result<User>.Failure("User not found", "USER_NOT_FOUND");
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Failed to get user: {ex.Message}", "GET_USER_ERROR");
            }
        }

        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                return user != null 
                    ? Result<User>.Success(user)
                    : Result<User>.Failure("User not found", "USER_NOT_FOUND");
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Failed to get user: {ex.Message}", "GET_USER_ERROR");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetActiveUsersAsync();
                return Result<IEnumerable<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure($"Failed to get active users: {ex.Message}", "GET_USERS_ERROR");
            }
        }

        public async Task<Result<IEnumerable<User>>> GetUsersByRoleAsync(string role)
        {
            try
            {
                var users = await _userRepository.GetByRoleAsync(role);
                return Result<IEnumerable<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure($"Failed to get users by role: {ex.Message}", "GET_USERS_ERROR");
            }
        }

        public async Task<Result<bool>> DeactivateUserAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return Result<bool>.Failure("User not found", "USER_NOT_FOUND");
                }

                user.IsActive = false;
                await _userRepository.UpdateAsync(user);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to deactivate user: {ex.Message}", "DEACTIVATE_ERROR");
            }
        }

        public async Task<Result<bool>> UpdateUserRoleAsync(int id, string newRole)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return Result<bool>.Failure("User not found", "USER_NOT_FOUND");
                }

                user.Role = newRole;
                await _userRepository.UpdateAsync(user);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update user role: {ex.Message}", "UPDATE_ROLE_ERROR");
            }
        }
    }
}
