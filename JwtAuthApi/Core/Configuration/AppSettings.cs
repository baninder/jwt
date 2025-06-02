namespace JwtAuthApi.Core.Configuration
{
    /// <summary>
    /// Application settings configuration
    /// </summary>
    public class AppSettings
    {
        public const string SectionName = "AppSettings";

        public string ApplicationName { get; set; } = "JWT Auth API";
        public string Version { get; set; } = "1.0.0";
        public bool EnableDetailedErrors { get; set; } = false;
        public bool EnableRequestLogging { get; set; } = true;
        public CorsSettings Cors { get; set; } = new();
    }

    /// <summary>
    /// CORS settings configuration
    /// </summary>
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; } = { "*" };
        public string[] AllowedMethods { get; set; } = { "GET", "POST", "PUT", "DELETE", "OPTIONS" };
        public string[] AllowedHeaders { get; set; } = { "*" };
        public bool AllowCredentials { get; set; } = true;
    }
}
