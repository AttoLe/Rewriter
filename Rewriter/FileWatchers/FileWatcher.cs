using Rewriter.Extensions;

namespace Rewriter.FileWatchers;

public class FileWatcher : FileSystemWatcher, IObservable<FileSystemEventArgs>
{
    private readonly HashSet<IObserver<FileSystemEventArgs>> _converters = [];
    private readonly ILogger<FileWatcher> _logger;
    
    public FileWatcher(ILogger<FileWatcher> logger, string path, IEnumerable<string>? filters = null)
    {
        _logger = logger;
        
        Path = path;
        NotifyFilter = NotifyFilters.FileName;
        EnableRaisingEvents = true;

        if (filters is null) return;
        foreach (var filter in filters)
        {
            Filters.Add('*' + filter);
        }

        Created += OnCreated;
    }

    public IDisposable Subscribe(IObserver<FileSystemEventArgs> observer)
    {
        if (_converters.Add(observer))
        {
            _logger.LogNewSubscription(observer.GetType().ToString(), this.GetType().ToString(), Path);
            return new Unsubscriber<FileSystemEventArgs>(_converters, observer);
        }

        _logger.LogAlreadySubscribed(observer.GetType().ToString(), this.GetType().ToString(), Path);
        return Unsubscriber.NullUnsubscriber;
    }
    
    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        //if check as workaround on possible double event raised error by FileSystemWatcher
        if (File.GetLastWriteTime(e.FullPath) == File.GetLastAccessTime(e.FullPath)) return;
        
        var converter = _converters.FirstOrDefault(conv =>
            conv.GetType().IsRightExtensionConverter(System.IO.Path.GetExtension(e.FullPath)));
        
        converter?.OnNext(e);
    }
}
