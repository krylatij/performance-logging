// See https://aka.ms/new-console-template for more information

using Autofac;
using BenchmarkDotNet.Running;
using PerformanceLogging;
using PerformanceLogging.DependencyInjection;
using Serilog;


try
{
   RunBenchmark();

   //await RunLocalAsync();

}
catch (Exception e)
{
    Log.Logger.Error(e, "Error occurred.");
}
finally
{
    Log.Logger.Information("done");
    Console.ReadLine();
}

void RunBenchmark(){
    BenchmarkRunner.Run<PerformanceLogging.BenchmarkDotNet>();
}

async Task RunLocalAsync()
{
    var container = ContainerProvider.BuildContainer();

    //var services = container.Resolve<IEnumerable<IService>>();

    var provider = container.Resolve<AlgorithmProviderDelegate>();

    var services = Enum.GetValues<AlgorithmType>().Select(x => provider(x));

    foreach (var service in services)
    {
        var t1 = service.GetTimeAsync();

        var t2 = service.GetTimeAsync(500);

        await Task.WhenAll(t1, t2);

        var t3 = service.GetTime();

        var t4 = service.GetTime(500);
    }
}

