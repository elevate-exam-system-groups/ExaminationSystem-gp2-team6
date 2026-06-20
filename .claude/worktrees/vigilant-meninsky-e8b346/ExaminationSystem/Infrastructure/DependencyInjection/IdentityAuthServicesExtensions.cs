using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Email;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Features.Authentication.SendOtp.Service;
using ExaminationSystem.Features.Authentication.Shared.Service;
using ExaminationSystem.Features.Authentication.Token.Service;
using ExaminationSystem.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ExaminationSystem.Extensions.Infrastructure;

public static class IdentityAuthServicesExtensions
{
    public static IServiceCollection AddIdentityAndAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity Configuration (Merged with Lockout settings)
        services.AddDataProtection();
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // Authentication & JWT Configuration
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        services.AddAuthorization();

        // Token & Auth Custom Services
        services.AddScoped<IHasherService, HasherService>();
        services.AddScoped<IGenerateTokenService, AccessTokenService>();
        services.AddScoped<IGenerateRefreshTokenService, RefreshTokenService>();

        // Otp & Email Service
        services.AddScoped<IGenerateOtpService, GenerateOtpService>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}