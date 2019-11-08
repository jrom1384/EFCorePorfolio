using EFCore.Common;
using EFCore.DataLayer;
using EFCore.DataLayer.EFClasses;
using EFCore.MVC.Constants;
using EFCore.MVC.Options;
using EFCore.ServiceLayer;
using EFCore.Utilities.SendGrid;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EFCore.MVC.Configurations
{
    public static class AppAuthenticationConfiguration
    {       
        public static void AddAppAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityAuthenticationConfiguration();
            services.AddFacebookAuthenticationConfiguration(configuration);
        }

        private static void AddIdentityAuthenticationConfiguration(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>()
           .AddDefaultUI(UIFramework.Bootstrap4)
           .AddEntityFrameworkStores<ApplicationDBContext>();

            //services.AddDefaultIdentity<IdentityUser>(options =>
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                //Require email confirmation
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.SlidingExpiration = true;
                options.Cookie.Name = ApplicationAuthenticationSchemes.DefaultAuthenticationScheme;
            });

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
            opt =>
            {
                //configure your other properties
                opt.LoginPath = "/Account/Login";
            });

            //SignInManager, UserManager, IdentityUser
            services.AddScoped<IAccountService, AccountService>();

            //Utilities
            services.AddScoped<IEmailSender, EmailSender>();
        }

        private static void AddFacebookAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            FacebookSettings facebookSettings = new FacebookSettings();
            configuration.GetSection(Sections.FacebookSettings).Bind(facebookSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = ApplicationAuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultAuthenticateScheme = ApplicationAuthenticationSchemes.DefaultAuthenticationScheme;
                options.DefaultSignInScheme = ApplicationAuthenticationSchemes.DefaultAuthenticationScheme;

                //options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            })
           .AddFacebook(facebookOptions =>
           {
               facebookOptions.AppId = facebookSettings.AppId;
               facebookOptions.AppSecret = facebookSettings.AppSecret;
               facebookOptions.CallbackPath = facebookSettings.CallbackPath;
           })
           .AddCookie(ApplicationAuthenticationSchemes.DefaultAuthenticationScheme);
        }
    }
}
