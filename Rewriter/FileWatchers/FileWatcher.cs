namespace Rewriter.FileWatchers;

public class FileWatcher : FileSystemWatcher, IObservable<FileSystemEventArgs>
{
    private readonly HashSet<IObserver<FileSystemEventArgs>> _converters = [];
    
    public FileWatcher(string path, IEnumerable<string>? filters = null)
    {
        Path = path;
        NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        EnableRaisingEvents = true;

        if (filters is null) return;
        
        foreach (var filter in filters)
        {
            Filters.Add(filter);
        }
        
        Created += (_, args) =>
        {
            foreach (var converter in _converters)
            {
                converter.OnNext(args);
            }
        };
    }
    
    public IDisposable Subscribe(IObserver<FileSystemEventArgs> observer)
    {
        _converters.Add(observer);
        return new Unsubscriber<FileSystemEventArgs>(_converters, observer);
    }
}
