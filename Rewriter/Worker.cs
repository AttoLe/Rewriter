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

    private readonly List<IDisposable> _subscriptions = [];
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
        DisposeSubscriptions();
        _fileWatcherFactory.OutOfRangeWatchersDispose(_inputOptions.Extensions);
        
        foreach (var path in _inputOptions.FolderPaths)
        {
            if(!_fileWatcherFactory.TryGetWatcher(path, out var watcher, _inputOptions.Extensions))
                continue;
                    
            foreach (var extension in _inputOptions.Extensions)
            {
                if (!_converterFactory.TryGetConverter(extension, out var converter)) continue;
                
                var subscription = watcher.Subscribe(converter);
                _subscriptions.Add(subscription);
            }
        }     
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.ForEach(sub => sub.Dispose());
        _subscriptions.Clear();
    }
    
    public override void Dispose()
    {
        DisposeSubscriptions();
    }
}