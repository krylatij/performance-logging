using System.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Serialization;
using Serilog;

namespace PerformanceLogging.Postsharp;

[PSerializable]
public class PostsharpTimerAttribute : OnMethodBoundaryAspect
{
    private const string Prefix = "Postsharp";

    public override void OnEntry(MethodExecutionArgs args)
    {
        var st = Stopwatch.StartNew();
        args.MethodExecutionTag = st;
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        var st = (Stopwatch)args.MethodExecutionTag;

        var message = MessageProvider.GetMessage(Prefix, args.Method.DeclaringType.Name, args.Method.Name, st.Elapsed,
            args.Arguments.ToArray());

        if (st.ElapsedMilliseconds > Constants.WarnDelayMilliseconds)
        {
            Log.Logger.Warning(message);
        }
        else
        {
            Log.Logger.Information(message);
        }
    }
}