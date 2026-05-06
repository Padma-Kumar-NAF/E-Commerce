
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace E_Commerce.Gateway
{
    public class Program
    {
        public async static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("ocelot.json");

            builder.Services.AddOcelot();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            await app.UseOcelot();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
