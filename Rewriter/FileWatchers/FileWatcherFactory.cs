using Rewriter.Extensions;

namespace Rewriter.FileWatchers;

public class FileWatcherFactory(IServiceProvider serviceProvider, ILogger<FileWatcherFactory> logger) : IDisposable
{
    private readonly Dictionary<string, FileWatcher> _watchers = [];

    public FileWatcher TryGetWatcher(string inputFolderPath, IEnumerable<string>? filters = null)
    {
        if (!_watchers.TryGetValue(inputFolderPath, out var existedWatcher))
        {
            return CreateNewWatcher(inputFolderPath, filters);
        }

        if (!Equals(existedWatcher.Filters, filters))
        {
            existedWatcher.Filters.Clear();
            foreach (var filter in filters ?? Enumerable.Empty<string>())
            {
                existedWatcher.Filters.Add(filter);
            }
        }
        
        logger.LogAlreadyCreatedWatcher(inputFolderPath);
        return existedWatcher;
    }

    private FileWatcher CreateNewWatcher(string path, IEnumerable<string>? filters = null)
    {
        var watcher = ActivatorUtilities.CreateInstance<FileWatcher>(serviceProvider, path, filters ?? Array.Empty<string>());
        _watchers.Add(path, watcher);
        
        logger.LogNewWatcherCreated(path);
        return watcher;
    }


    public void OutOfRangeWatchersDispose(IEnumerable<string> paths)
    {
       var extraPaths = _watchers.Keys.Where(path => !paths.Contains(path)).ToList();
       
       extraPaths.ForEach(path =>
       {
           _watchers[path].EnableRaisingEvents = false;
           _watchers[path].Dispose();
           
           _watchers.Remove(path);
           logger.LogExtraWatcherDisposed(path);
       });
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
