using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;

        public PublicController(ILogger<PublicController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]        public ActionResult GetPublicInfo()
        {
            var info = new
            {
                ApplicationName = "JWT Auth API Enterprise",
                Version = "2.0.0",
                Description = "Enterprise-level .NET Web API with JWT authentication using Strategy, Factory, Repository, and Specification patterns",
                Architecture = new
                {
                    Patterns = new[]
                    {
                        "Strategy Pattern - JWT token operations",
                        "Factory Pattern - Token strategy creation",
                        "Repository Pattern - Data access abstraction",
                        "Specification Pattern - Query logic encapsulation",
                        "Result Pattern - Operation outcome handling",
                        "Unit of Work Pattern - Transaction management",
                        "Dependency Injection - Service lifetime management"
                    },
                    Features = new[]
                    {
                        "Structured logging with correlation IDs",
                        "Request/Response middleware pipeline",
                        "Fluent validation with detailed error messages",
                        "Enterprise-level error handling",
                        "Configuration options pattern",
                        "CORS policy management"
                    }
                },
                SupportedEndpoints = new[]
                {
                    "POST /api/auth/login - Login with email/password",
                    "POST /api/auth/register - Register new user",
                    "POST /api/auth/validate-token - Validate JWT token",
                    "GET /api/user/profile - Get user profile (requires authentication)",
                    "GET /api/user/dashboard - Get dashboard data (requires authentication)",
                    "GET /api/admin/users - Get all users (requires Admin role)",
                    "GET /api/admin/analytics - Get analytics (requires Admin role)",
                    "POST /api/admin/system-settings - Update settings (requires Admin role)"
                },
                TestCredentials = new
                {
                    Admin = new { Email = "jane.smith@example.com", Password = "admin123" },
                    User = new { Email = "john.doe@example.com", Password = "password123" }
                }
            };

            return Ok(info);
        }

        [HttpGet("health")]
        public ActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            });
        }
    }
}
