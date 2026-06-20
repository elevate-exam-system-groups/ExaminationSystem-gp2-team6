using ExaminationSystem.Common.Behaviors;
using FluentValidation;

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

        // FluentValidation — scan all validators in this assembly
        services.AddValidatorsFromAssemblyContaining<Program>();

        // MediatR & Pipeline Behaviors (order matters: Validation → Caching → Handler)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        return services;
    }
}