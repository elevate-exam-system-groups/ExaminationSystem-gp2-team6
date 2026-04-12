using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Contracts.Seed;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using ExaminationSystem.Infrastructure.Persistence.DB.Seed;

namespace ExaminationSystem.Extensions.Infrastructure;

public static class AddInfrastructureServicesExtensions
{
    public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // Data Seeding
        services.AddScoped<IDataSeeding, DataSeeding>();
        
        return services;
    }
}