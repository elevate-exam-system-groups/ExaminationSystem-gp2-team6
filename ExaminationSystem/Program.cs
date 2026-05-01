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

            // 1. Add Core Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Your custom Swagger schema configuration
                options.CustomSchemaIds(type => type.FullName);
            });

            // 2. Add Layer Specific Services (Dependency Injection)
            builder.Services.AddApplicationServices();
            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddIdentityAndAuthServices(builder.Configuration);

            var app = builder.Build();

            // 3. Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            // Auth Pipeline (Authentication MUST be before Authorization)
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // 4. Data Seeding
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