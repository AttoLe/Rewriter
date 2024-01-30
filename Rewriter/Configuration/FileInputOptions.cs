namespace Rewriter.Configuration;

public sealed class FileInputOptions
{
    public const string Key = "FileInput";
    
    public IEnumerable<string> FolderPaths { get; set; }
    public IEnumerable<string> Extensions { get; set; }
}