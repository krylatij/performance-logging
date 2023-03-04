using Autofac;
using BenchmarkDotNet.Attributes;
using PerformanceLogging.DependencyInjection;

namespace PerformanceLogging;

public class BenchmarkDotNet
{
    private IContainer _container;

    private IService _autofacService;

    private IService _methodTimerFodyService;

    private IService _postsharpService;

    [GlobalSetup]
    public void Setup()
    {
        _container = ContainerProvider.BuildContainer(false);

        var serviceProvider = _container.Resolve<AlgorithmProviderDelegate>();

        _autofacService = serviceProvider(AlgorithmType.Autofac);
        _methodTimerFodyService = serviceProvider(AlgorithmType.MethodTimerFody);
        _postsharpService = serviceProvider(AlgorithmType.Postsharp);
    }

    [GlobalCleanup]
    public void Cleanup() => _container.Dispose();

    [Benchmark]
    public void AutofacSync() => _autofacService.GetTime();

    [Benchmark]
    public async Task AutofacAsync() => await _autofacService.GetTimeAsync();

    [Benchmark]
    public void MethodTimerSync() => _methodTimerFodyService.GetTime();

    [Benchmark]
    public async Task MethodTimerAsync() => await _methodTimerFodyService.GetTimeAsync();

    [Benchmark]
    public void PostsharpSync() => _postsharpService.GetTime();

    [Benchmark]
    public async Task PostsharpAsync() => await _postsharpService.GetTimeAsync();
}