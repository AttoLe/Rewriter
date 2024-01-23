namespace Rewriter.Logger;

public class FileLoggerOption
{
    public string FolderPath { get; set; } = string.Empty;
    public LogLevel MinimaLogLevel { get; set; } = LogLevel.Information;

}