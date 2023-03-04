namespace PerformanceLogging;

public interface IService
{
    Task<DateTime> GetTimeAsync(int delay = 0);
    DateTime GetTime(int delay = 0);
}