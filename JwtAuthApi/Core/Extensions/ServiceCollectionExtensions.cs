using FluentValidation;
using JwtAuthApi.Configuration;
using JwtAuthApi.Core.Configuration;
using JwtAuthApi.Core.Factories;
using JwtAuthApi.Core.Interfaces;
using JwtAuthApi.Core.Logging;
using JwtAuthApi.Core.Middleware;
using JwtAuthApi.Core.Repositories;
using JwtAuthApi.Core.Services;
using JwtAuthApi.Core.UnitOfWork;
using JwtAuthApi.Core.Validators;
using JwtAuthApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtAuthApi.Core.Extensions
{
    /// <summary>
    /// Extension methods for service registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all application services with dependency injection
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionName));            // Logging
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

            // Repositories and Unit of Work
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

            // Factories
            services.AddScoped<ITokenStrategyFactory, JwtTokenStrategyFactory>();

            // Services
            services.AddScoped<IJwtService, EnterpriseJwtService>();
            services.AddScoped<IUserService, EnterpriseUserService>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

            return services;
        }

        /// <summary>
        /// Configures JWT authentication
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            
            if (jwtSettings == null)
                throw new InvalidOperationException("JWT settings are not configured");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        /// <summary>
        /// Configures CORS with application settings
        /// </summary>
        public static IServiceCollection AddApplicationCors(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(AppSettings.SectionName).Get<AppSettings>() ?? new AppSettings();

            services.AddCors(options =>
            {                options.AddPolicy("DefaultPolicy", policy =>
                {
                    if (appSettings.Cors.AllowedOrigins.Contains("*"))
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    }
                    else
                    {
                        policy.WithOrigins(appSettings.Cors.AllowedOrigins);
                        
                        if (appSettings.Cors.AllowedMethods.Contains("*"))
                        {
                            policy.AllowAnyMethod();
                        }
                        else
                        {
                            policy.WithMethods(appSettings.Cors.AllowedMethods);
                        }

                        if (appSettings.Cors.AllowedHeaders.Contains("*"))
                        {
                            policy.AllowAnyHeader();
                        }
                        else
                        {
                            policy.WithHeaders(appSettings.Cors.AllowedHeaders);
                        }

                        if (appSettings.Cors.AllowCredentials)
                        {
                            policy.AllowCredentials();
                        }
                    }
                });
            });

            return services;
        }
    }

    /// <summary>
    /// Extension methods for application builder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the application middleware pipeline
        /// </summary>
        public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(AppSettings.SectionName).Get<AppSettings>() ?? new AppSettings();

            if (appSettings.EnableRequestLogging)
            {
                app.UseRequestLogging();
            }

            app.UseCors("DefaultPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
