using Rewriter.Extensions;

namespace Rewriter.FileWatchers;

public class Unsubscriber<T>
    (ICollection<IObserver<T>> observers, IObserver<T> observer, ILogger logger, IObservable<T> source, string path) 
    : Unsubscriber
{
    public override void Dispose()
    {
        logger.LogUnsubscribed(observer.GetType().Name, source.GetType().Name, path);
        observers.Remove(observer);
    }
}

public class Unsubscriber : IDisposable
{
    public static readonly Unsubscriber NullUnsubscriber = new();
    
    public virtual void Dispose()
    {
    }
}
