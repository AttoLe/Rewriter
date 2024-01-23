namespace Rewriter.Logger;

public class FileLogger(string name, Func<FileLoggerOption> getCurrentConfig) : ILogger
{

    private Func<ILogger, string, IDisposable?> scope = LoggerMessage.DefineScope<string>("Scope {scopeName}");

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state));
        }

        return scope(this, "");
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var option = getCurrentConfig();

        throw new NotImplementedException();

        //TODO write it
    }

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel >= getCurrentConfig().MinimaLogLevel;
}
