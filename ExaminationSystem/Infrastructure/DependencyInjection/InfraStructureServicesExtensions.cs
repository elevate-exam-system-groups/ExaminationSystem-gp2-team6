using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Infrastructure.Persistence.Context;
using ExaminationSystem.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ExaminationSystem.Infrastructure.DependencyInjection;

public static class InfraStructureServicesExtensions
{
    public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.LogTo(log => Debug.WriteLine(log), LogLevel.Information).EnableSensitiveDataLogging(true);// Enable sensitive data logging for debugging purposes
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Default tracking behavior set to NoTracking

        });
        
        // Identity
        services.AddDataProtection();
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        // Data Seeding
        services.AddScoped<IDataSeeding, DataSeeding>();
        
        return services;
    }
}