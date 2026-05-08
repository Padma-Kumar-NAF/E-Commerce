using Auth.Application.Services;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Application.Interfaces;
using User.Application.Settings;
using User.Infrastructure;

namespace User.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IAuthService, AuthService>();

            var jwtSettings = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtSettings>()!;

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            builder.Services.AddAuthorization();

            

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.MapGet("/secrets", async (IConfiguration configuration) =>
            {
                try
                {
                    var vaultUri = configuration["KeyVault:VaultUri"];

                    Console.WriteLine($"Vault URI: {vaultUri}");

                    var secretsClient = new SecretClient(
                        new Uri(vaultUri!),
                        new DefaultAzureCredential());

                    KeyVaultSecret secret =
                        await secretsClient.GetSecretAsync("JwtSignature");

                    Console.WriteLine($"Secret Value: {secret.Value}");

                    return Results.Ok(secret.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.BadRequest(ex.Message);
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
