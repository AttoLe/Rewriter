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
        _fileWatcherFactory.OutOfRangeWatchersDispose(_inputOptions.FolderPaths);
        
        foreach (var path in _inputOptions.FolderPaths)
        {
            var watcher = _fileWatcherFactory.TryGetWatcher(path, _inputOptions.Extensions);
                    
            foreach (var extension in _inputOptions.Extensions)
            {
                if (!_converterFactory.TryGetConverter(extension, out var converter)) continue;
                
                var subscription = watcher.Subscribe(converter);
                _subscriptions.Add(subscription);
            }
        }

        Console.WriteLine("\n");
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