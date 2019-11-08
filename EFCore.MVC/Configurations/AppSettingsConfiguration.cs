using EFCore.Common;
using EFCore.MVC.Options;
using EFCore.Utilities.SendGrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MVC.Configurations
{
    public static class AppSettingsConfiguration
    {
        public static void AddAppSettingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var pageSettingSections = configuration.GetSection(Sections.PageSettings);
            services.Configure<PageSettings>(pageSettingSections);

            var sendGridSettingSection = configuration.GetSection(Sections.SendGridSettings);
            services.Configure<SendGridSettings>(sendGridSettingSection);

            var facebookSettingSection = configuration.GetSection(Sections.FacebookSettings);
            services.Configure<FacebookSettings>(facebookSettingSection);
        }
    }
}
