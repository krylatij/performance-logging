namespace PerformanceLogging.Postsharp;

public class PostsharpService : ServiceBase
{
    [PostsharpTimer]
    public override async Task<DateTime> GetTimeAsync(int delay = 0)
    {
        return await base.GetTimeAsync(delay);
    }

    [PostsharpTimer]
    public override DateTime GetTime(int delay = 0)
    {
        return base.GetTime(delay);
    }
}