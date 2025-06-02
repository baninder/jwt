using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires valid JWT token
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("profile")]
        public ActionResult GetProfile()
        {
            try
            {
                var userId = User.FindFirst("user_id")?.Value;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var firstName = User.FindFirst("first_name")?.Value;
                var lastName = User.FindFirst("last_name")?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                var profile = new
                {
                    Id = userId,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = role,
                    Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, new { message = "An error occurred while getting profile" });
            }
        }

        [HttpGet("dashboard")]
        public ActionResult GetDashboard()
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                var dashboardData = new
                {
                    WelcomeMessage = $"Welcome, {userName}!",
                    Role = role,
                    LastLogin = DateTime.UtcNow,
                    Features = role == "Admin" 
                        ? new[] { "User Management", "Reports", "Settings", "Analytics" }
                        : new[] { "Profile", "Basic Reports" }
                };

                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data");
                return StatusCode(500, new { message = "An error occurred while getting dashboard data" });
            }
        }
    }
}
