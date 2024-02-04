namespace Rewriter.FileWatchers;

public class FileWatcher : FileSystemWatcher, IObservable<string>
{
    private readonly HashSet<IObserver<string>> _converters = [];
    
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
    }
    
    public IDisposable Subscribe(IObserver<string> observer)
    {
        if (_converters.Add(observer))
        {
            Created += (_, args) =>
            {
                foreach (var converter in _converters)
                {
                    converter.OnNext(args.FullPath);
                }
            };
        }

        return new Unsubscriber<string>(_converters, observer);
    }
}


//TODO check this and probably rewrite
//The ArrivalsMonitor class includes the Subscribe and Unsubscribe methods.
//The Subscribe method enables the class to save the IDisposable implementation returned by the call to
//Subscribe to a private variable. The Unsubscribe method enables the class to unsubscribe from notifications
//by calling the provider's Dispose implementation.

internal sealed class Unsubscriber<T> : IDisposable
{
    private readonly ISet<IObserver<T>> _observers;
    private readonly IObserver<T> _observer;

    internal Unsubscriber(
        ISet<IObserver<T>> observers,
        IObserver<T> observer) => (_observers, _observer) = (observers, observer);

    public void Dispose() => _observers.Remove(_observer);
}