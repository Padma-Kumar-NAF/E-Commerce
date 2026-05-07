using System.Text;
using Auth.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}


//using Auth.Application.Services;
//using User.Application.Interfaces;
//using User.Infrastructure;

//namespace User.API
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();
//            builder.Services.AddInfrastructure(builder.Configuration);
//            builder.Services.AddScoped<IAuthService, AuthService>();
//            builder.Services.AddOpenApi();

//            var app = builder.Build();

//            if (app.Environment.IsDevelopment())
//            {
//                app.MapOpenApi();
//            }
//            app.UseSwagger();

//            app.UseSwaggerUI();

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}
