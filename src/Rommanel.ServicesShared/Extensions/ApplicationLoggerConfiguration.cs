using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Configuration;

public static class ApplicationLoggerConfiguration
{
    public static void AddLoggerConfiguration(this IHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console(
                outputTemplate: "[{Level:u4}] |{SourceContext,30}({EventId})| {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateBootstrapLogger();

        host.UseSerilog();
    }
}