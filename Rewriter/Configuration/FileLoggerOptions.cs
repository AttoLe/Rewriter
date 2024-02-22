namespace Rewriter.Configuration;

public sealed class FileLoggerOptions
{
    public const string Key = "Logging";
    
    public string FolderPath { get; set; }
    public bool UseSeparateFiles { get; set; }
    
    public LogLevelOptions FileLogger { get; set; }
}

public class LogLevelOptions
{
    public LogLevel Default { get; set; }
}