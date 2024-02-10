using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Logger;

public class FileLogger: ILogger
{
    private Func<ILogger, string, IDisposable?> _scope = LoggerMessage.DefineScope<string>("Scope {scopeName}");
    private FileLoggerOptions _options;
    private readonly string _name;

    public FileLogger(string name, IOptionsMonitor<FileLoggerOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(updatedValue => _options = updatedValue);
        _name = name;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state));
        }
        
        return _scope(this, _name);
        
        //TODO change it??
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            Console.WriteLine($"{logLevel} -- {formatter(state, exception)}");
        }
        //_name is category
        //TODO write it
    }

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel >= _options.MinimaLogLevel;
}