using Datadog.Trace;
using Datadog.Trace.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace template.Api.Infrastructure.Tracing
{
	/// <summary>
	/// Implements <see cref="IServiceCollection"/> extensions for ease of configuring the tracing in the Startup.cs file.
	/// </summary>
	public static class TracingServicesExtension
    {
        public static void AddDistributedTracing(this IServiceCollection services)
        {
            var settings = TracerSettings.FromDefaultSources();

            settings.Integrations["AdoNet"].Enabled = false;

            var tracer = new Tracer(settings);

            // set the global tracer
            Tracer.Instance = tracer;
        }
    }
}
