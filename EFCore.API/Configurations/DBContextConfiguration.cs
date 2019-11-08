using EFCore.API.Constants;
using EFCore.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.API.Configurations
{
    public static class DBContextConfiguration
    {
        public static void AddDBContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStrings.CompanyEFCoreDBContext));
            });
        }
    }
}
