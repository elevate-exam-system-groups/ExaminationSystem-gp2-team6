using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Contracts.Seed;
using ExaminationSystem.Extensions.Infrastructure;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Core Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });

            // Dependency Injection
            builder.Services.AddApplicationServices();
            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddIdentityAndAuthServices(builder.Configuration);

            var app = builder.Build();

            // HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            // Auth Pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Data Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetRequiredService<IDataSeeding>();

                await seeder.DataSeedAsync();
                await seeder.IdentityDataSeedAsync();
            }

            app.Run();
        }
    }
}