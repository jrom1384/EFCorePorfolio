using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace EFCore.MVC.Configurations
{
    public static class ExceptionHandlerConfiguration
    {
        public static void AddAExceptionHandlerConfiguration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Errors/GlobalExceptionLevelHandler");
            }
            else
            {
                app.UseExceptionHandler("/Errors/GlobalExceptionLevelHandler");
            }
        }
    }
}
