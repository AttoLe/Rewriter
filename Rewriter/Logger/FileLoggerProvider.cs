using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Logger;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private FileLoggerOptions _currentConfig;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new FileLogger(name, GetCurrentConfig));

    private FileLoggerOptions GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}