using JwtAuthApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only Admin role can access
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var userList = users.Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.Role,
                    u.CreatedAt,
                    u.IsActive
                }).ToList();

                return Ok(userList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, new { message = "An error occurred while getting users" });
            }
        }

        [HttpGet("analytics")]
        public ActionResult GetAnalytics()
        {
            try
            {
                var currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
                
                var analytics = new
                {
                    TotalUsers = 150,
                    ActiveUsers = 142,
                    NewUsersThisMonth = 23,
                    LastUpdated = DateTime.UtcNow,
                    UpdatedBy = currentUser,
                    SystemStats = new
                    {
                        ServerUptime = TimeSpan.FromHours(168),
                        ApiCalls = 15647,
                        ErrorRate = 0.02
                    }
                };

                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analytics");
                return StatusCode(500, new { message = "An error occurred while getting analytics" });
            }
        }

        [HttpPost("system-settings")]
        public ActionResult UpdateSystemSettings([FromBody] object settings)
        {
            try
            {
                var adminUser = User.FindFirst(ClaimTypes.Name)?.Value;
                
                // Simulate updating system settings
                _logger.LogInformation("System settings updated by {AdminUser}", adminUser);
                
                return Ok(new 
                { 
                    message = "System settings updated successfully",
                    updatedBy = adminUser,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system settings");
                return StatusCode(500, new { message = "An error occurred while updating settings" });
            }
        }
    }
}
