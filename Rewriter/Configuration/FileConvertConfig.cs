namespace Rewriter.Configuration;

public class FileConvertConfig
{
    public const string Key = "FileConversion";
    
    public IEnumerable<string> InputFolderPathes { get; set; }
    public string OutputFolderPath { get; set; }
}