namespace Rewriter.FileWatchers;

public class Unsubscriber<T>(ICollection<IObserver<T>> observers, IObserver<T> observer) : Unsubscriber
{
    public override void Dispose()
    {
        Console.WriteLine("subscription disposed");
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
