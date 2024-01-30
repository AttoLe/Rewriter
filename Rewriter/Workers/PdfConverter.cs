namespace Rewriter.Workers;

public abstract class PdfConverter : IConverter
{
    public abstract void ConvertFile(string fullPath);

    public string ConvertPath(string fullOldPath, string newFolderPath) =>
        newFolderPath + "/" + Path.ChangeExtension(Path.GetFileName(fullOldPath), ".pdf");
}