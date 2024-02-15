using Microsoft.Extensions.Logging.Console;

namespace Rewriter.Configuration;

public sealed class FileLoggerOptions
{
    public const string Key = "Logging";
    
    public string FolderPath { get; set; }
    public bool UseSeparateFiles { get; set; }
    
    public LogLevelOptions LogLevel { get; set; }
}

public class LogLevelOptions
{
    public LogLevel Default { get; set; }
    public LogLevel FileLogger { get; set; }
}