using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMediaAPI.Core.Interfaces;
using SocialMediaAPI.Core.Services;
using SocialMediaAPI.DataAccess;
using SocialMediaAPI.DataAccess.Entities;
using SocialMediaAPI.DataAccess.Repositories;
using System.Text;

namespace SocialMediaAPI.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddDbContext<SocialMediaDbContext>(optionsBuilder =>
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("SocialMediaDb")));

            services.AddAutoMapper(assemblies);
            services.AddValidatorsFromAssemblies(assemblies);
            services.AddMemoryCache();

            services.AddScoped<IRepository<User, Guid>, Repository<User, Guid>>();
            services.AddScoped<IRepository<Role, int>, Repository<Role, int>>();
            services.AddScoped<IRepository<Country, long>, Repository<Country, long>>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<ICountryService, CountryService>();
        }

        public static void ConfigureJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = configuration.GetSection("Jwt");
            var secretKey = jwtConfiguration["Secret"]!;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfiguration["ValidIssuer"],
                        ValidAudience = jwtConfiguration["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }
    }
}
