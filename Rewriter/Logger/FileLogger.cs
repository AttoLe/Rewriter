using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Logger;

public class FileLogger: ILogger, IDisposable
{
    private readonly Func<ILogger, string, IDisposable?> _scope = LoggerMessage.DefineScope<string>("Scope {scopeName}");
    private FileLoggerOptions _options;
    private readonly string _name;
    private readonly IDisposable? _optionChange;
    private string _path;
    
    public FileLogger(string name, IOptionsMonitor<FileLoggerOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue;
        _name = name;
        _path = CalcPath();
        
        _optionChange = optionsMonitor.OnChange(updatedValue =>
        {
            _options = updatedValue;
            _path = CalcPath();
        });
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state));
        }
        
        return _scope(this, _name);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        //no isEnable check cause it is done before log invocation on LoggerExtensions
        //method though compile-time logging source generator  
        File.AppendAllLines(_path, new []{$"{logLevel} -- {DateTime.Now} -- {formatter(state, exception)}"});
    }

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel >= _options.LogLevel.FileLogger;

    private string CalcPath()
    {
        return _options.UseSeparateFiles 
            ? Path.Combine(_options.FolderPath, _name + ".log") 
            : Path.Combine(_options.FolderPath, "log.log");
    }
    
    public void Dispose()
    {
        _optionChange?.Dispose();
    }
}