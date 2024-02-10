namespace Rewriter.Extensions;

static partial class LoggerExtensions
{
    [LoggerMessage(0, LogLevel.Information, "New subscription is created. {listener} subscriber for {source} on {path}")]
    public static partial void LogNewSubscription(this ILogger logger, string listener, string source, string path);
    
    [LoggerMessage(0, LogLevel.Information, "{listener} is already subscribed for {source} on {path}")]
    public static partial void LogAlreadySubscribed(this ILogger logger, string listener, string source, string path);
    
    
    [LoggerMessage(0, LogLevel.Information, "New file watcher is created for {path}")]
    public static partial void LogNewWatcherCreated(this ILogger logger, string path);
    
    [LoggerMessage(0, LogLevel.Information, "File watcher for {path} is already created")]
    public static partial void LogAlreadyCreatedWatcher(this ILogger logger, string path);
    
    [LoggerMessage(0, LogLevel.Information, "File watcher for {path} is disposed")]
    public static partial void LogExtraWatcherDisposed(this ILogger logger, string path);
    
    
    [LoggerMessage(0, LogLevel.Error, "No converter for {extension}")]
    public static partial void LogNoConverterError(this ILogger logger, string extension);
    
    [LoggerMessage(0, LogLevel.Information, "New converter for {extension} is created ({converterTypeName})")]
    public static partial void LogNewConverterCreated(this ILogger logger, string extension, string converterTypeName);
    
    [LoggerMessage(0, LogLevel.Information, "File watcher for {extension} is already created ({converterTypeName})")]
    public static partial void LogAlreadyCreatedConverter(this ILogger logger, string extension, string converterTypeName);
    
    
    [LoggerMessage(0, LogLevel.Information, "File from {path} path is converting)")]
    public static partial void LogFileConverting(this ILogger logger, string path);
    
    [LoggerMessage(0, LogLevel.Information, "File from {path} path is successfully converted)")]
    public static partial void LogFileConverted(this ILogger logger, string path);
    
    
 

    public static void LogConfigurationChanged(this ILogger logger, string configurationName)
    {
        logger.LogInformation($"Configuration '{configurationName}' changed.");
    }

}
