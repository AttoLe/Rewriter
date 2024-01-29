using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Logger;

public sealed class FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> monitor) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, FileLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new FileLogger(name, monitor));

    public void Dispose()
    {
        _loggers.Clear();
    }
}