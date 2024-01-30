using System.Reflection;
using Rewriter.Attributes;
using Rewriter.Configuration;
using Rewriter.FileWatchers;
using Rewriter.Workers;

namespace Rewriter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidationOptions<TOptions>(this IServiceCollection serviceCollection,
        string sectionName) where TOptions : class
    {
        serviceCollection.AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .ValidateFluentValidation().ValidateOnStart();

        return serviceCollection;
    }

    public static IServiceCollection AddConverterServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        
        var options = configuration.GetSection(FileInputOptions.Key).Get<FileInputOptions>();
        serviceCollection.AddSingleton<FileWatcherProvider>();
        
        foreach (var extension in options!.Extensions)
        {
            var converterType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass)
                .FirstOrDefault(t =>
                    t.GetCustomAttribute<ExtensionAttribute>() is { Extensions: { } extensions } &&
                    extensions.Contains(extension)
                );
            
            if (converterType is null)
            {
                //logger error
                continue;
            }
            
            var existedService = serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(IConverter) && descriptor.ImplementationType == converterType);

            Console.WriteLine(converterType);
            if (existedService is not null) continue;
            serviceCollection.AddTransient(typeof(IConverter), converterType);
            //ActivatorUtilities.CreateInstance(provider, converterType));
            
            var workerType = typeof(Worker<>).MakeGenericType(converterType);
            Console.WriteLine(workerType);
            var existedService2 = serviceCollection.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(IHostedService) && descriptor.ImplementationType == workerType);
            
            Console.WriteLine(workerType);
            if (existedService2 is not null) continue;
            serviceCollection.AddHostedService(provider => (IHostedService) ActivatorUtilities.CreateInstance(provider, workerType));
            //logger success on that extension
        }

        return serviceCollection;
    }
}