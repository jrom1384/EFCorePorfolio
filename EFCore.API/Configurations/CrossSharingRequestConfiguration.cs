using Microsoft.AspNetCore.Builder;

namespace EFCore.API.Configurations
{
    public static class CrossSharingRequestConfiguration
    {
        public static void ConfigureCORS(this IApplicationBuilder app)
        {
            //To test type the following at google chrome console while currently on other domain/site.
            //fetch("https://localhost:44339/api/departments").then(a => a.text()).then(console.log)

            //This will allow Cross Sharing Request globally
            app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //app.UseCors(x => x.WithOrigins("https://SiteUsingThisAPI.com")
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());               
        }
    }
}
