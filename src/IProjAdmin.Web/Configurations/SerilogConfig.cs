using Serilog.Events;
using Serilog;

namespace IProjAdmin.Web.Configurations
{
    public static class SerilogConfig
    {
        public static void ConfigureSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            hostBuilder.UseSerilog(Log.Logger);

            hostBuilder.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(configuration));
        }
    }
}
