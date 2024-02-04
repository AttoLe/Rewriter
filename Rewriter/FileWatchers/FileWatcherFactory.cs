using System.Diagnostics.CodeAnalysis;

namespace Rewriter.FileWatchers;

public class FileWatcherFactory(ILogger<FileWatcherFactory> logger) : IDisposable
{
    private readonly Dictionary<string, FileWatcher> _watchers = [];

    public bool TryGetWatcher(string inputFolderPath,  [MaybeNullWhen(false)] out FileWatcher watcher, IEnumerable<string>? filters = null)
    {
        if (!_watchers.TryGetValue(inputFolderPath, out var existedWatcher))
        {
            var result = CreateNewWatcher(inputFolderPath, filters);
            watcher = result;
            
            return result is null;
        }

        if (!Equals(existedWatcher.Filters, filters))
        {
            existedWatcher.Filters.Clear();
            foreach (var filter in filters ?? Enumerable.Empty<string>())
            {
                existedWatcher.Filters.Add(filter);
            }
        }
        
        watcher = existedWatcher;
        return true;
    }

    private FileWatcher? CreateNewWatcher(string path, IEnumerable<string>? filters = null)
    {
        if (!Directory.Exists(path))
        {
            //logger
            return null;
        }

        var watcher = new FileWatcher(path, filters);
        _watchers.Add(path, watcher);
        return watcher;
    }

    public void Dispose()
    {
        foreach (var (_,  watcher) in _watchers)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
        
        _watchers.Clear();
    }
}
