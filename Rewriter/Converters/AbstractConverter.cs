using Microsoft.Extensions.Options;
using Rewriter.Configuration;

namespace Rewriter.Converters;

public abstract class AbstractConverter : IObserver<string>
{
    protected FileOutputOptions Options;

    protected AbstractConverter(IOptionsMonitor<FileOutputOptions> optionsMonitor)
    {
        Options = optionsMonitor.CurrentValue;
        optionsMonitor.OnChange(option => Options = option);
    }

    protected abstract void ConvertFile(string fullPath);

    protected static string ConvertPath(string fullOldPath, string newFolderPath) =>
        newFolderPath + "/" + Path.ChangeExtension(Path.GetFileName(fullOldPath), ".pdf");
    
    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(string value)
    {
        ConvertFile(value);
    }
}