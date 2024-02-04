namespace Rewriter.FileWatchers;

public class Unsubscriber<T>(ICollection<IObserver<T>> observers, IObserver<T> observer) : IDisposable
{
    public void Dispose()
    {
        observers.Remove(observer);
    }
}