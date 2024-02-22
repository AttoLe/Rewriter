using Rewriter.Extensions;

namespace Rewriter.FileWatchers;

public class FileWatcherFactory(IServiceProvider serviceProvider, ILogger<FileWatcherFactory> logger) : IDisposable
{
    private readonly Dictionary<string, FileWatcher> _watchers = [];

    public FileWatcher GetWatcher(string inputFolderPath, IEnumerable<string>? extensions = null)
    {
        var filters = extensions is null ? [] : extensions.Select(ext => "*" + ext).ToList();
        
        if (!_watchers.TryGetValue(inputFolderPath, out var existedWatcher))
        {
            return CreateNewWatcher(inputFolderPath, filters);
        }

        if (!existedWatcher.Filters.SequenceEqual(filters))
        {
            existedWatcher.Filters.Clear();
            foreach (var filter in filters)
            {
                existedWatcher.Filters.Add(filter);
            }

            logger.LogWatcherExtensionsChanged(inputFolderPath);
        }

        logger.LogAlreadyCreatedWatcher(inputFolderPath);
        return existedWatcher;
    }

    private FileWatcher CreateNewWatcher(string path, IEnumerable<string> filters)
    {
        var watcher = ActivatorUtilities.CreateInstance<FileWatcher>(serviceProvider, path, filters);
        _watchers.Add(path, watcher);
        
        logger.LogNewWatcherCreated(path);
        return watcher;
    }


    public void ExtraWatchersDispose()
    {
       var extraPaths = _watchers.Where(pair => !pair.Value.IsActive()).Select(pair => pair.Key).ToList();
       
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
