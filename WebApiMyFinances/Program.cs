using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.Core.Services;
using WebApiMyFinances.Infrastructure.Repositories.EntityFramework;
using WebApiMyFinances.Security;
using WebApiMyFinances.Security.Jwt;
using WebApiMyFinances.Shared.Middlewares;
using WebApiMyFinances.WebApi.Mapping;

namespace WebApiMyFinances
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseConsoleLifetime();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    string secretKey = configuration["AuthOptions:Key"] ??
                        throw new Exception("Секретный ключ не определен");
                    string issuer = configuration["AuthOptions:Issuer"] ??
                        throw new Exception("Секретный ключ не определен");
                    string audience = configuration["AuthOptions:Audience"] ??
                        throw new Exception("Секретный ключ не определен");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidIssuer = issuer,
                        ValidAudience = audience
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["auth-token"];

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(DefaultMappingProfile));

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUserApiService, UserApiService>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserApiRoleService, UserApiRoleService>();

            builder.Services.AddDbContext<DatabaseContext>();

            var app = builder.Build();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
