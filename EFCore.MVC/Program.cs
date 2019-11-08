using EFCore.Utilities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EFCore.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = SerilogHelper.SetBasicConfiguration();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .ConfigureLogging((logging) =>
                 {
                     logging.ClearProviders();
                 })
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
