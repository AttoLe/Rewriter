using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Rewriter.Configuration;

namespace Rewriter.Logger;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <FileLoggerOptions, FileLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
    {
        if (configure is null)
            throw new NullReferenceException();

        builder.AddFileLog();
        builder.Services.Configure(configure);

        return builder;
    }
}