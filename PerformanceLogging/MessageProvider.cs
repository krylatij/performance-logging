namespace PerformanceLogging;

internal static class MessageProvider
{
    public static string GetMessage(string loggerType, string typeName, string methodName, TimeSpan executionTime, params object[] arguments)
    {
        if (arguments.Length == 0)
        {
            return $"{typeName}.{methodName}() elapsed in {executionTime}";
        }

        return $"[{loggerType}]{typeName}.{methodName}({string.Join(',',arguments)}) elapsed in {executionTime}";
    }
}