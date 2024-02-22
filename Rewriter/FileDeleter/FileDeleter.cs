using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.Extensions;

namespace Rewriter.FileDeleter;

public class FileDeleter(IOptionsMonitor<FileInputOptions> monitor, ILogger<FileDeleter> logger) : IFileDeleter
{
    public bool TryDeleteFile(string path)
    {
        if (!monitor.CurrentValue.DeleteOldFile)
            return true;

        //add recursion deletion through new prop on FileInputOptions
        
        return DeleteOneFile(path);
    }

    private bool DeleteOneFile(string path)
    {
        try
        {
            File.Delete(path);
            logger.LogFileDeleted(path);
            return true;
        }
        catch (Exception e)
        {
            logger.LogFileDeletionError(path, e.Message);
            return false;
        }
    }
    
}
