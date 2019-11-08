using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.IO;

namespace EFCore.Utilities
{
    public class SerilogHelper
    {
        public static Logger SetBasicConfiguration()
        {
           var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            return new LoggerConfiguration()
                .WriteTo.ColoredConsole(restrictedToMinimumLevel: LogEventLevel.Verbose)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
