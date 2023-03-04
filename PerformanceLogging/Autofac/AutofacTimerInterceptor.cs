using System.Diagnostics;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace PerformanceLogging.Autofac;

public class AutofacTimerInterceptor : IInterceptor
{
    private readonly ILogger<AutofacTimerInterceptor> _logger;

    private const string Prefix = "Autofac";
        
    public AutofacTimerInterceptor(ILogger<AutofacTimerInterceptor> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var start = Stopwatch.StartNew();
        invocation.Proceed();

        if (invocation.Method.ReturnType.BaseType == typeof(Task))
        {
            //todo:  cancellation token!
            invocation.ReturnValue = WatchAsync(start, (dynamic)invocation.ReturnValue, invocation);
        }
        else
        {
            PostProcess(start, invocation);
        }
    }

    private void PostProcess(Stopwatch stopWatch, IInvocation invocation)
    {
        stopWatch.Stop();

        var message = MessageProvider.GetMessage(Prefix, invocation.Method.DeclaringType.Name, invocation.Method.Name, stopWatch.Elapsed, invocation.Arguments);

        if (stopWatch.ElapsedMilliseconds > Constants.WarnDelayMilliseconds)
        {
            _logger.LogWarning(message);
        }
        else
        {
            _logger.LogInformation(message);
        }
    }

    private async Task<T> WatchAsync<T>(Stopwatch stopWatch, Task<T> methodExecution, IInvocation invocation)
    {
        try
        {
            return await methodExecution.ConfigureAwait(false);
        }
        finally
        {
            PostProcess(stopWatch, invocation);
        }
    }
}