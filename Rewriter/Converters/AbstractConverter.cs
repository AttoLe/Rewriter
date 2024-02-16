using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.FileDeleter;

namespace Rewriter.Converters;

public abstract class AbstractConverter
    (IOptionsMonitor<FileOutputOptions> optionsMonitor, IFileDeleter fileDeleter) 
    : IObserver<FileSystemEventArgs>, IDisposable
{
    protected abstract void ConvertFile(string fullPath);

    public abstract void OnCompleted();

    public abstract void OnError(Exception error);
    
    public void OnNext(FileSystemEventArgs value)
    {
        ConvertFile(value.FullPath);
        TryDelete(value.FullPath);
    }
    
    private bool TryDelete(string fullPath) 
        => fileDeleter.TryDeleteFile(fullPath);

    protected string ConvertToNewPath(string fullOldPath)
    {
        return Path.Combine(optionsMonitor.CurrentValue.FolderPath,
            Path.ChangeExtension(Path.GetFileName(fullOldPath), ".pdf"));
    }
    
    public abstract void Dispose();
}