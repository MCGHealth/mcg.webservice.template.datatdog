using System;
using template.Api.Infrastructure.Configuration;
using Datadog.Trace;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace template.Api.Infrastructure.Logging
{
	/// <summary>
	/// An extension method that adds the enricher to the Serilog configuration.
	/// </summary>
	public static class LogEnricherExtensions
	{
		/// <summary>
		/// Adds custom log enrichment to include values from the http context as well
		/// as permit correlation between Datadog APM and log entries.
		/// </summary>
		/// <param name="enrichmentConfiguration"></param>
		/// <returns></returns>
		public static LoggerConfiguration WithRequestEnricher(this LoggerEnrichmentConfiguration enrichmentConfiguration)
		{
			if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
			return enrichmentConfiguration.With<RequestEnricher>();
		}
	}

	/// <summary>
	/// Enriches the logs with common information useful for analyzing performance, as well as
	/// correlating logs to distributed traces.
	/// </summary>
	public class RequestEnricher : ILogEventEnricher
	{
		private readonly IHttpContextAccessor ctxAccessor;

		public RequestEnricher() : this(new HttpContextAccessor()) { }

		public RequestEnricher(IHttpContextAccessor contextAccessor)
		{
			ctxAccessor = contextAccessor;
		}

		/// <summary>
		/// Performs the actual enrichment of the logs.
		/// </summary>
		/// <param name="le">The <see cref="LogEvent"/> that is to be enriched.</param>
		/// <param name="pf">The <see cref="ILogEventPropertyFactory"/> used to enrich the event.</param>
		public void Enrich(LogEvent le, ILogEventPropertyFactory pf)
		{
			var ctx = ctxAccessor.HttpContext;
			le.AddOrUpdateProperty(pf.CreateProperty("http_method", ctx.Request.Method));
			le.AddOrUpdateProperty(pf.CreateProperty("path", ctx.Request.Path));
			le.AddOrUpdateProperty(pf.CreateProperty("host_name", Environment.MachineName));
			le.AddOrUpdateProperty(pf.CreateProperty("app_name", AppSettings.ServiceName));
			le.AddOrUpdateProperty(pf.CreateProperty("app_ver", AppSettings.AppVersion));
			le.AddOrUpdateProperty(pf.CreateProperty("prod_ver", AppSettings.ProductVersion));
			le.AddOrUpdateProperty(pf.CreateProperty("dd.trace_id", CorrelationIdentifier.TraceId.ToString()));
			le.AddOrUpdateProperty(pf.CreateProperty("dd.span_id", CorrelationIdentifier.SpanId.ToString()));
		}
	}
}
