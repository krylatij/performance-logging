using System.Security.Cryptography.X509Certificates;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PerformanceLogging.Autofac;
using PerformanceLogging.MethodTimer.Fody;
using PerformanceLogging.Postsharp;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PerformanceLogging.DependencyInjection;

internal static class ContainerProvider
{
    public static IContainer BuildContainer(bool addLogging = true)
    {
        var f = new AutofacServiceProviderFactory();
        var services = new ServiceCollection();

        services.AddLogging(x =>
        {
            x.ClearProviders();

            if (addLogging)
            {
                var loggerConfiguration = new LoggerConfiguration()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

                Log.Logger = loggerConfiguration.CreateLogger();

                x.AddSerilog(Log.Logger);
            }
        });

        services.AddSingleton<IService, MethodTimerFodyService>();
         services.AddSingleton<IService, PostsharpService>();

        //services.AddSingleton<MethodTimerFodyService>();
        //services.AddSingleton<PostsharpService>();

        services.AddSingleton<AlgorithmProviderDelegate>(x => algorithm =>
        {
            var services = x.GetService<IEnumerable<IService>>();

            return algorithm switch
            {
                AlgorithmType.Autofac => services.Single(x => x.GetType().FullName == "Castle.Proxies.IServiceProxy"),
                AlgorithmType.MethodTimerFody => services.OfType<MethodTimerFodyService>().Single(),
                AlgorithmType.Postsharp => services.OfType<PostsharpService>().Single(),
                _ => throw new NotImplementedException($"Unknown algorithm {algorithm}")
            };
        });

        var builder = f.CreateBuilder(services);

        builder.RegisterType<AutofacService>().SingleInstance().As<IService>().EnableInterfaceInterceptors().InterceptedBy(typeof(AutofacTimerInterceptor));
        builder.RegisterType<AutofacTimerInterceptor>();


        

        //builder.Register<AlgorithmProviderDelegate>(x => algorithm =>
        //{
        //    var services = x.Resolve<IEnumerable<IService>>();

        //    return algorithm switch
        //    {
        //        AlgorithmType.Autofac => x.Resolve<AutofacService>(),
        //        AlgorithmType.MethodTimerFody => x.Resolve<MethodTimerFodyService>(),
        //        AlgorithmType.Postsharp => x.Resolve<PostsharpService>(),
        //        _ => throw new NotImplementedException($"Unknown algorithm {algorithm}")
        //    };
        //}).SingleInstance();

        return builder.Build();
    }
}