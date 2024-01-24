namespace Rewriter.Configuration;

public class FileConvertOptions
{
    public const string Key = "FileConversion";
    
    public IEnumerable<string> InputFolderPaths { get; set; }
    public string OutputFolderPath { get; set; }
}