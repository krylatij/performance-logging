using MethodTimer;

namespace PerformanceLogging.MethodTimer.Fody;

public class MethodTimerFodyService : ServiceBase
{
    [Time]
    public override async Task<DateTime> GetTimeAsync(int delay = 0)
    {
        return await base.GetTimeAsync(delay);
    }

    [Time]
    public override DateTime GetTime(int delay = 0)
    {
        return base.GetTime(delay);
    }
}