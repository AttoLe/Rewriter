using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Converters;

public abstract class AbstractConverter(IOptions<FileOutputOptions> options)
    : IObserver<FileSystemEventArgs>, IDisposable
{
    protected readonly FileOutputOptions Options = options.Value;
    
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