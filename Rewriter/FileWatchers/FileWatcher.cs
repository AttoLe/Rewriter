namespace Rewriter.FileWatchers;

public class FileWatcher : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private readonly Action<string> _convertAction;

    public FileWatcher(string folderPath, IEnumerable<string> filters, Action<string> convert)
    {
        _convertAction = convert;

        _watcher = new FileSystemWatcher
        {
            Path = folderPath,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        foreach (var filter in filters)
        {
            _watcher.Filters.Add(filter);
        }

        _watcher.Created += OnFileCreated;
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e) => _convertAction(e.FullPath);

    public void Dispose()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Created -= OnFileCreated;
        _watcher.Dispose();
    }
}
