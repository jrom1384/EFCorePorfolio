using EFCore.Utilities.MVC;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MVC.Configurations
{
    public static class MVCProviderConfiguration
    {
        public static void AddMVCProviderConfiguration(this IServiceCollection services)
        {
            services.AddMvc(option => option.AddStringTrimmingProvider());
        }
    }
}
