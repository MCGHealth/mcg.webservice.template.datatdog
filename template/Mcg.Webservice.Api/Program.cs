using System;
using System.Configuration;
using Mcg.Webservice.Api.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Mcg.Webservice.UnitTests")]
namespace Mcg.Webservice.Api
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Title = $"{typeof(Program).Assembly.GetName().Name} RESTful API";
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((ctx, config) =>
				{
					config.MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
						  .MinimumLevel.Override("System", LogEventLevel.Fatal)
						  .Enrich.FromLogContext()
						  .WriteTo.Console(new JsonLogFormatter());

					_ = GetLogLevel() switch
					{
						LogEventLevel.Debug => config.MinimumLevel.Debug(),
						LogEventLevel.Verbose => config.MinimumLevel.Verbose(),
						LogEventLevel.Information => config.MinimumLevel.Information(),
						LogEventLevel.Warning => config.MinimumLevel.Warning(),
						LogEventLevel.Error => config.MinimumLevel.Error(),
						_ => throw new ConfigurationErrorsException()
					};
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


		private static LogEventLevel GetLogLevel()
		{
			var loglevel = Environment.GetEnvironmentVariable("APP_LOG_LEVEL");

			if (string.IsNullOrWhiteSpace(loglevel))
			{
				throw new ConfigurationErrorsException($"Missing value for APP_LOG_LEVEL.");
			}

			var (ok, level) = loglevel.ToEnum<LogEventLevel>();

			if (!ok)
			{
				throw new ConfigurationErrorsException($"Invalid logging level: {loglevel}.");
			}

			return level;
		}
	}
}
