using EFCore.Common;
using EFCore.Razor.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.Razor.Configurations
{
    public static class AppSettingsConfiguration
    {
        public static void AddAppSettingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var pageSettingSections = configuration.GetSection(Sections.PageSettings);
            services.Configure<PageSettings>(pageSettingSections);
        }
    }
}
