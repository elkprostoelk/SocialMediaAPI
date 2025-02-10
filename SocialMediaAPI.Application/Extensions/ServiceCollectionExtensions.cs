using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.DataAccess;

namespace SocialMediaAPI.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SocialMediaDbContext>(optionsBuilder =>
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("SocialMediaDb")));
        }
    }
}
