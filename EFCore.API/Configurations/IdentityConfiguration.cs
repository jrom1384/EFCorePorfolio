using EFCore.DataLayer;
using EFCore.DataLayer.EFClasses;
using EFCore.ServiceLayer;
using EFCore.Utilities.SendGrid;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.API.Configurations
{
    public static class IdentityConfiguration
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>()
             .AddEntityFrameworkStores<ApplicationDBContext>();

            //SignInManager, UserManager, IdentityUser
            services.AddScoped<IAccountService, AccountService>();

            //Utilities
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
