using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExamCore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddControllers();

            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            
            

            return services;
        }
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddMapperConfiguration(this IServiceCollection services)
        {
            //services.AddAutoMapper(/*typeof(CommonInfo).Assembly*/);
            return services;
        }
        public static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthenticationConfiguration(configuration);
            return services;
        }
    }
}
