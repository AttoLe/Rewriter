namespace Rewriter.FileWatchers;

public class FileWatcherProvider(ILogger<FileWatcherProvider> logger) : IDisposable
{
    private readonly List<FileWatcher> _watchers = [];

    public void AddWatcher(string inputFolderPath, IEnumerable<string> filters, Action<string> convert)
    {
        if (!Directory.Exists(inputFolderPath))
        {
            //logger
            return;
        }

        _watchers.Add(new FileWatcher(inputFolderPath, filters, convert));
    }

    public void Dispose()
    {
        _watchers.ForEach(watcher => watcher.Dispose());
        _watchers.Clear();
    }
}
