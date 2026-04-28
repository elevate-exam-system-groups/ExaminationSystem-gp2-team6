
using ExaminationSystem.Contracts.Seed;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Extensions.Infrastructure;
using MediatR;

namespace ExaminationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container..

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Dependency Injection Services
            
            // InfraStructure (Database - Seeding)
            builder.Services.AddInfraStructureServices(builder.Configuration);
            
            // Identity + Token
            builder.Services.AddIdentityServices(builder.Configuration);
            
            // MediatR
            builder.Services.AddMediatR(typeof(Program).Assembly);
            
            #endregion
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
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
