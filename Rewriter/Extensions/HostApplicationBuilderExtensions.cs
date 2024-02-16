using System.Diagnostics;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.Converters;
using Rewriter.FileDeleter;
using Rewriter.FileWatchers;
using Rewriter.Validation;

namespace Rewriter.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder AddWindowServiceInjection(this HostApplicationBuilder builder)
    {
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = ".NET File Rewriter";
        });
        return builder;
    }
    
    public static HostApplicationBuilder AddValidatedConfiguration(this HostApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();
        
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (string.IsNullOrWhiteSpace(environment)) environment = "Production";
        builder.Configuration.AddJsonFile($"appsettings.{environment}.json", false, true);
        
        builder.AddFluentValidationOptions<FileInputOptions>(FileInputOptions.Key);
        builder.AddFluentValidationOptions<FileOutputOptions>(FileOutputOptions.Key);
        builder.AddFluentValidationOptions<FileLoggerOptions>(FileLoggerOptions.Key);

        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        
        return builder;
    }
    
    public static HostApplicationBuilder AddWorkerComponent(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<FileWatcherFactory>();
        builder.Services.AddSingleton<ConverterFactory>();
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddSingleton<IFileDeleter, FileDeleter.FileDeleter>();

        return builder;
    }

    public static HostApplicationBuilder AddFileLogging(this HostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders(); 
        builder.Logging.AddFileLog();
        
        return builder;
    }
    
    public static HostApplicationBuilder SetCurrentDirectory(this HostApplicationBuilder builder)
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Enumerable.Range(0, 3).ToList().ForEach(_ => directory = Directory.GetParent(directory!)!.ToString());
        
        builder.Configuration.SetBasePath(directory!);
        return builder;
    }
    
    private static HostApplicationBuilder AddFluentValidationOptions<TOptions>(this HostApplicationBuilder builder,
        string sectionName) where TOptions : class
    {
        builder.Services.Configure<TOptions>(builder.Configuration.GetSection(sectionName));

        builder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
            new FluentValidateOptions<TOptions>(serviceProvider, string.Empty));

        return builder;
    }
}