using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Converters;

public abstract class AbstractConverter : IObserver<FileSystemEventArgs>, IDisposable
{
    protected FileOutputOptions Options;

    protected AbstractConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor)
    {
        Options = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(option => Options = option);
    }

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