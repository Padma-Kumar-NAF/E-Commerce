using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.Infrastructure;
using Product.Infrastructure.Data;

namespace Product.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Initialize Cosmos DB (create database and container if they don't exist)
            if (app.Environment.IsDevelopment())
            {
                try
                {
                    var initializer = new CosmosDbInitializer(app.Configuration);
                    await initializer.InitializeAsync();
                    Console.WriteLine("Cosmos DB initialized successfully.");

                    // Optionally seed sample data
                    Console.WriteLine("\nDo you want to seed sample data? This will add 5 sample products.");
                    Console.WriteLine("Seeding sample data...");
                    await SampleDataSeeder.SeedFromConfiguration(app.Configuration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing Cosmos DB: {ex.Message}");
                    Console.WriteLine("Make sure the Cosmos DB Emulator is running.");
                }

                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}