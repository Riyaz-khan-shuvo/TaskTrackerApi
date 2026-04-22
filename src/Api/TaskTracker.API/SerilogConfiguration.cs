using Serilog;

namespace TaskTracker.API
{
    public static class SerilogConfiguration
    {
        // Return type explicitly Serilog.ILogger to avoid ambiguity
        public static Serilog.ILogger CreateBootstrapLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();
        }
        public static void ConfigureLogger(HostBuilderContext ctx, LoggerConfiguration lc)
        {
            lc.MinimumLevel.Information()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .WriteTo.File(
                  path: "Logs/TaskTracker-.txt",
                  rollingInterval: RollingInterval.Day,
                  shared: true,
                  buffered: false,          // <--- Disable buffered writes
                  rollOnFileSizeLimit: true,
                  retainedFileCountLimit: 30
              );
        }


        //public static void ConfigureLogger(Microsoft.Extensions.Hosting.HostBuilderContext ctx, LoggerConfiguration lc)
        //{
        //    lc.MinimumLevel.Debug()
        //      .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        //      .Enrich.FromLogContext()
        //      .WriteTo.Console()
        //      .WriteTo.File(
        //          path: "Logs/TaskTracker-.log",
        //          rollingInterval: RollingInterval.Day,
        //          rollOnFileSizeLimit: true,
        //          retainedFileCountLimit: 30,
        //          buffered: true,
        //          shared: true);
        //}
    }
}
