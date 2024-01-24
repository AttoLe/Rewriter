namespace Rewriter.Configuration;

public class FileConvertConfig
{
    public const string Key = "FileConversion";
    
    public IEnumerable<string> InputFolderPaths { get; set; }
    public string OutputFolderPath { get; set; }
}