using ExaminationSystem.Contracts.Email;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Contracts.Otp;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Entities.Email;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;
using ExaminationSystem.Features.Authentication.SendOtp.Service;
using ExaminationSystem.Features.Authentication.Shared.Service;
using ExaminationSystem.Features.Authentication.Token.Service;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Extensions.Infrastructure;

public static class AddIdentityServicesExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity
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
        
        // Token Services
        services.AddScoped<IHasherService, HasherService>();
        services.AddScoped<IGenerateTokenService, AccessTokenService>();
        services.AddScoped<IGenerateRefreshTokenService, RefreshTokenService>();
        
        // Otp - Email Service
        services.AddScoped<IGenerateOtpService, GenerateOtpService>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }
}