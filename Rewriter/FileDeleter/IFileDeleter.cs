namespace Rewriter.FileDeleter;

public interface IFileDeleter
{
    public bool TryDeleteFile(string path);
}