using ExaminationSystem.Common.Behaviors;

namespace ExaminationSystem.Extensions.Infrastructure;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(Program).Assembly);
        });

        // MediatR & Pipeline Behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        return services;
    }
}