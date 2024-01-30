namespace Rewriter.Workers;

public interface IConverter
{
    void ConvertFile(string fullPath);

    string ConvertPath(string fullOldPath, string newFolderPath);
}