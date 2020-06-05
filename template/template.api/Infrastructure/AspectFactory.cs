using System;
using template.Api.Infrastructure.Configuration;
using template.Api.Infrastructure.Logging;
using template.Api.Infrastructure.Tracing;

namespace template.Api.Infrastructure
{
	public class AspectFactory
	{
		internal static double TickPerMicrosecond => (double)(TimeSpan.TicksPerMillisecond) * 1000D;

		internal static Type VoidTaskResult => Type.GetType("System.Threading.Tasks.VoidTaskResult");

		internal static IAppSettings Settings;

		public static object GetInstance(Type aspectType)
		{
			if (aspectType == typeof(LogAspect))
			{
				return new LogAspect();
			}

			if (aspectType == typeof(TraceAspect))
			{
				return new TraceAspect();
			}

			throw new ApplicationException($"Unknown aspect type: {aspectType}.");
		}
	}
}
