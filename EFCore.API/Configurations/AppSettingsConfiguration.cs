using EFCore.API.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.API.Configurations
{
    public static class AppSettingsConfiguration
    {
        public static void AddAppSettingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection(Sections.JwtSettings);
            services.Configure<JwtSettings>(jwtSettingsSection);
        }
    }
}
