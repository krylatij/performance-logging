using System.Reflection;

namespace PerformanceLogging.MethodTimer.Fody;

public static class MethodTimeLogger
{
    public const string Prefix = "MethodTimer.Fody";

    public static void Log(MethodBase methodBase, TimeSpan elapsed, string originalMessage)
    {
        var message = MessageProvider.GetMessage(Prefix, methodBase.DeclaringType.Name, methodBase.Name, elapsed, "<...>");

        if (elapsed.TotalMilliseconds > Constants.WarnDelayMilliseconds)
        {
            Serilog.Log.Logger.Warning(message);
        }
        else
        {
            Serilog.Log.Logger.Information(message);
        }
    }
}