namespace Rewriter.Configuration;

public class FileLoggerOptions
{
    public string FolderPath { get; set; } = string.Empty;
    public LogLevel MinimaLogLevel { get; set; } = LogLevel.Information;

}