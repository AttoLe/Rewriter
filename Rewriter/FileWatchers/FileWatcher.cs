using System.Reflection;
using Rewriter.Attributes;

namespace Rewriter.FileWatchers;

public class FileWatcher : FileSystemWatcher, IObservable<FileSystemEventArgs>
{
    private readonly HashSet<IObserver<FileSystemEventArgs>> _converters = [];

    public FileWatcher(string path, IEnumerable<string>? filters = null)
    {
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
        _converters.Add(observer);
        
        return new Unsubscriber<FileSystemEventArgs>(_converters, observer);
    }
    
    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var converter = _converters.FirstOrDefault(conv =>
            conv.GetType().GetCustomAttribute<ExtensionAttribute>() is { Extensions: { } extensions } &&
            extensions.Contains(System.IO.Path.GetExtension(e.FullPath)))!;
        
        if (File.GetLastWriteTime(e.FullPath) == File.GetLastAccessTime(e.FullPath)) return;
        
        converter?.OnNext(e);
    }
}
