using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace Rewriter.Logger;

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private FileLoggerOption _currentConfig;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

    public FileLoggerProvider(IOptionsMonitor<FileLoggerOption> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new FileLogger(name, GetCurrentConfig));

    private FileLoggerOption GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}