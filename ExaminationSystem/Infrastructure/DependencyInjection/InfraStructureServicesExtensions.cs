using System.Diagnostics;
using ExaminationSystem.Contracts.Seed;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Infrastructure.Persistence.Context;
using ExaminationSystem.Infrastructure.Persistence.Repositories;
using ExaminationSystem.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.DependencyInjection;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                   .EnableSensitiveDataLogging(false);
        });

        // Caching
        services.AddMemoryCache();

        // Data Seeding
        services.AddScoped<IDataSeeding, DataSeeding>();

        // Repositories & UnitOfWork
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        return services;
    }
}