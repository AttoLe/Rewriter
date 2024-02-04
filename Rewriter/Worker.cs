using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.Converters;
using Rewriter.FileWatchers;

namespace Rewriter;

public class Worker : BackgroundService
{
    private readonly ConverterFactory _converterFactory;
    private readonly FileWatcherFactory _fileWatcherFactory;
    private FileInputOptions _inputOptions;

    public Worker(IOptionsMonitor<FileInputOptions> inputOptionsMonitor, FileWatcherFactory fileWatcherFactory, ConverterFactory converterFactory)
    {
        _inputOptions = inputOptionsMonitor.CurrentValue;
        inputOptionsMonitor.OnChange(options =>
        {
            _inputOptions = options;
            Execute();
        });

        _fileWatcherFactory = fileWatcherFactory;
        _converterFactory = converterFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Execute();
        return Task.CompletedTask;
    }

    private void Execute()
    {
        foreach (var path in _inputOptions.FolderPaths)
        {
            if(!_fileWatcherFactory.TryGetWatcher(path, out var watcher, _inputOptions.Extensions))
                continue;
                    
            foreach (var extension in _inputOptions.Extensions)
            {
                if (_converterFactory.TryGetConverter(extension, out var converter))
                    watcher.Subscribe(converter);
            }
        }     
    }
}