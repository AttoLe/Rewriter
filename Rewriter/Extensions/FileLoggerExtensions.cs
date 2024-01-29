using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Rewriter.Logger;

namespace Rewriter.Extensions;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFileLog(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
        
        return builder;
    }
}