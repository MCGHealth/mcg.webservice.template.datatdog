using System;
using System.Reflection;
using System.Threading.Tasks;
using AspectInjector.Broker;
using Datadog.Trace;
using template.Api.Infrastructure.Configuration;

namespace template.Api.Infrastructure.Tracing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    [Injection(typeof(TraceAspect))]
    public sealed class TraceAttribute : Attribute { }

    /// <summary>
    /// Adds distributed tracing to the decorated method.
    /// </summary>
    [Aspect(AspectInjector.Broker.Scope.Global, Factory = typeof(AspectFactory))]
    public class TraceAspect
    {
        private static readonly MethodInfo AsyncHandler = typeof(TraceAspect)
            .GetMethod(nameof(TraceAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo SyncHandler = typeof(TraceAspect)
            .GetMethod(nameof(TraceAspect.WrapSync), BindingFlags.NonPublic | BindingFlags.Static);

        [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle(
            [Argument(Source.Type)] Type owningType,
            [Argument(Source.Name)] string targetName,
            [Argument(Source.Target)] Func<object[], object> targetFunc,
            [Argument(Source.Arguments)] object[] inputArgs,
            [Argument(Source.ReturnType)] Type returnType)
        {
            object[] invocationArgs = {
                $"{owningType.Name.ToLower()}_{targetName.ToLower()}",
                targetFunc,
                inputArgs,
            };

            if (typeof(Task).IsAssignableFrom(returnType))
            {
                var syncResultType = returnType.IsConstructedGenericType ? returnType.GenericTypeArguments[0] : AspectFactory.VoidTaskResult;
                return AsyncHandler.MakeGenericMethod(syncResultType).Invoke(this, invocationArgs);
            }
            else
            {
                returnType = returnType == typeof(void) ? typeof(object) : returnType;
                return SyncHandler.MakeGenericMethod(returnType).Invoke(this, invocationArgs);
            }
        }

        private static T WrapSync<T>(string eventName, Func<object[], object> target, object[] args)
        {

            using (var scope = Tracer.Instance.StartActive("web.request"))
            {
                var span = scope.Span;
                span.Type = SpanTypes.Custom;
                span.ResourceName = eventName;
                span.SetTag("machine", Environment.MachineName);
                span.SetTag("service", AppSettings.ServiceName);
                try
                {
                    var result = (T)target(args);
                    return result;
                }
                catch (Exception ex)
                {
                    span.SetException(ex);
                    throw;
                }
            }
        }

        private static async Task<T> WrapAsync<T>(string eventName, Func<object[], object> target, object[] args)
        {
            using (var scope = Tracer.Instance.StartActive("web.request"))
            {
                var span = scope.Span;
                span.Type = SpanTypes.Custom;
                span.ResourceName = eventName;
                span.SetTag("machine", Environment.MachineName);
                span.SetTag("service", AppSettings.ServiceName);

                try
                {
                    var result = await (Task<T>)target(args);
                    return result;
                }
                catch (Exception ex)
                {
                    span.SetException(ex);
                    throw;
                }
            }
        }
    }
}
