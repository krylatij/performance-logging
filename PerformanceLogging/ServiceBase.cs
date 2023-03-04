namespace PerformanceLogging;

public abstract class ServiceBase : IService
{
    public virtual async Task<DateTime> GetTimeAsync(int delay = 0)
    {
        if (delay > 0)
        {
            await Task.Delay(delay);
        }
           
        return DateTime.UtcNow;
    }

    public virtual DateTime GetTime(int delay = 0)
    {
        if (delay > 0)
        {
            Thread.Sleep(delay);
        }
            
        return DateTime.UtcNow;
    }
}