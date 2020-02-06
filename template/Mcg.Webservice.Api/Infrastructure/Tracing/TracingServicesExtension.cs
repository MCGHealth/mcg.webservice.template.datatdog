using Datadog.Trace.OpenTracing;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;

namespace Mcg.Webservice.Api.Infrastructure.Tracing
{
	/// <summary>
	/// Implements <see cref="IServiceCollection"/> extensions for ease of configuring the tracing in the Startup.cs file.
	/// </summary>
	public static class TracingServicesExtension
    {
		public static void AddDistributedTracing(this IServiceCollection services)
		{
			ITracer tracer = OpenTracingTracerFactory.CreateTracer();

			services.AddSingleton<ITracer>(tracer);

			GlobalTracer.Register(tracer);
		}
	}
}
