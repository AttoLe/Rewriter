namespace Rewriter.Configuration;

public sealed class FileLoggerOptions
{
    public const string Key = "Logging";
    
    public string FolderPath { get; set; }
    public bool UseSeparateFiles { get; set; }
    public LogLevel MinimaLogLevel { get; set; } = LogLevel.Information;
}