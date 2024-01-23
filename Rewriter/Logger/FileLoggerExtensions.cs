using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace Rewriter.Logger;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <FileLoggerOption, FileLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder, Action<FileLoggerOption> configure)
    {
        if (configure is null)
            throw new NullReferenceException();

        builder.AddFileLog();
        builder.Services.Configure(configure);

        return builder;
    }
}