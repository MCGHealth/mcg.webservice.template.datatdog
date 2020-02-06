using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mcg.Webservice.UnitTests")]
namespace Mcg.Webservice.Api
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, config) =>
                {
                    _ = config
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
                })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.ConfigureKestrel(serverOptions =>
                     {
                         serverOptions.AddServerHeader = true;
                     })
                     .UseStartup<Startup>();
                 })
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddEnvironmentVariables("APP_");
                 });
    }
}
