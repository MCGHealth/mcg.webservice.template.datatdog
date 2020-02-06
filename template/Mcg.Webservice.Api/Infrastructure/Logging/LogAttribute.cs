using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using AspectInjector.Broker;
using Mcg.Webservice.Api.Infrastructure.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace Mcg.Webservice.Api.Infrastructure.Logging
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	[Injection(typeof(LogAspect))]
	public sealed class LogAttribute : Attribute { }

	/// <summary>
	/// Adds logging to the decorated method.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[Aspect(Scope.Global, Factory = typeof(AspectFactory))]
	public class LogAspect
	{
		internal const string INF_LOG_TEMPLATE = "{host_name} {process_name} {type_name} {method} {elapsed_\u03BCs:0.0000} ";
		internal const string ERR_LOG_TEMPLATE = INF_LOG_TEMPLATE + " {arguments} {error_type} {error_message} {error_stack_trace}";

		internal static ILogger Log { get; set; } = Serilog.Log.Logger;

		private static readonly MethodInfo AsyncHandler = typeof(LogAspect)
			.GetMethod(nameof(LogAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

		private static readonly MethodInfo SyncHandler = typeof(LogAspect)
			.GetMethod(nameof(LogAspect.Wrap), BindingFlags.NonPublic | BindingFlags.Static);

		private static T Wrap<T>(string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var sw = Stopwatch.StartNew();

			try
			{
				var result = (T)target(args);
				WriteInformation(owningTypeName, targetName, sw.ElapsedTicks);
				return result;
			}
			catch (Exception e)
			{
				var ex = GetRootException(e);
				WriteError(owningTypeName, targetName, args, sw.ElapsedTicks, ex);
				throw;
			}
		}

		private static async Task<T> WrapAsync<T>(string owningTypeName, string targetName, Func<object[], object> target, object[] args)
		{
			var sw = Stopwatch.StartNew();

			try
			{
				var result = await (Task<T>)target(args);
				WriteInformation(owningTypeName, targetName, sw.ElapsedTicks);
				return result;
			}
			catch (Exception e)
			{
				var ex = GetRootException(e);
				WriteError(owningTypeName, targetName, args, sw.ElapsedTicks, ex);
				throw;
			}
		}

		private static void WriteInformation(string owningTypeName, string methodName, long duration)
		{
			if (methodName.Contains("_around_"))
			{
				return;
			}

			double elapsed = (double)duration / AspectFactory.TickPerMicrosecond;

			Log.Information(
				INF_LOG_TEMPLATE
				, Environment.MachineName
				, AppSettings.ServiceName
				, owningTypeName
				, methodName
				, elapsed
			);
		}

		private static void WriteError(string owningTypeName, string methodName, object[] args, long duration, Exception exception)
		{
			if (methodName.Contains("_around_"))
			{
				return;
			}

			double elapsed = (double)duration / AspectFactory.TickPerMicrosecond;

			Log.Error(
				ERR_LOG_TEMPLATE
				, Environment.MachineName
				, AppSettings.ServiceName
				, owningTypeName
				, methodName
				, elapsed
				, HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(args))
				, exception.GetType().FullName
				, exception.Message
				, HttpUtility.JavaScriptStringEncode(exception.StackTrace)
			);

			return;

		}

		internal static Exception GetRootException(Exception ex)
		{
			if (ex is TargetInvocationException)
			{
				GetRootException(ex.InnerException);
			}

			return ex;
		}

		[Advice(Kind.Around, Targets = Target.Method)]
		public object Handle(
			[Argument(Source.Type)] Type owningType,
			[Argument(Source.Name)] string targetName,
			[Argument(Source.Target)] Func<object[], object> targetFunc,
			[Argument(Source.Arguments)] object[] inputArgs,
			[Argument(Source.ReturnType)] Type returnType)
		{
			string typeName = owningType.Name.ToLower();

			object[] invocationArgs =
				{
					typeName,
					targetName.ToLower(),
					targetFunc,
					inputArgs,
				};

			if (typeof(Task).IsAssignableFrom(returnType))
			{
				var syncResultType = returnType.IsConstructedGenericType
					? returnType.GenericTypeArguments[0]
					: AspectFactory.VoidTaskResult;

				return AsyncHandler.MakeGenericMethod(syncResultType).Invoke(this, invocationArgs);
			}

			returnType = returnType == typeof(void)
				? typeof(object)
				: returnType;

			return SyncHandler.MakeGenericMethod(returnType).Invoke(this, invocationArgs);
		}
	}
}
