using JwtAuthApi.Core.Logging;
using System.Diagnostics;

namespace JwtAuthApi.Core.Middleware
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, IAppLogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];

            // Log request
            _logger.LogInformation(
                "Request {RequestId}: {Method} {Path} started",
                requestId,
                context.Request.Method,
                context.Request.Path);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Request {RequestId}: {Method} {Path} failed with exception",
                    requestId,
                    context.Request.Method,
                    context.Request.Path);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                
                // Log response
                _logger.LogInformation(
                    "Request {RequestId}: {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms",
                    requestId,
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }

    /// <summary>
    /// Extension method for registering request logging middleware
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
