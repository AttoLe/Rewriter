namespace Rewriter.Converters;

public abstract class AbstractConverter : IObserver<FileSystemEventArgs>, IDisposable
{
    protected abstract void ConvertFile(string fullPath);

    protected static string ConvertPath(string fullOldPath, string newFullFolderPath) =>
        newFullFolderPath + "\\" + Path.ChangeExtension(Path.GetFileName(fullOldPath), ".pdf");

    public abstract void OnCompleted();

    public abstract void OnError(Exception error);
    
    public void OnNext(FileSystemEventArgs value)
    {
        ConvertFile(value.FullPath);
    }

    public abstract void Dispose();
}