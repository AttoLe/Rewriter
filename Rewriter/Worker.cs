using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.Converters;
using Rewriter.Extensions;
using Rewriter.FileWatchers;

namespace Rewriter;

public class Worker : BackgroundService
{
    private readonly ConverterFactory _converterFactory;
    private readonly FileWatcherFactory _fileWatcherFactory;
    private FileInputOptions _inputOptions;
    private readonly List<IDisposable> _subscriptions = [];
    private readonly ILogger<Worker> _logger;
    private readonly IDisposable? _optionsChange;

    public Worker(IOptionsMonitor<FileInputOptions> inputOptionsMonitor, FileWatcherFactory fileWatcherFactory,
        ConverterFactory converterFactory, ILogger<Worker> logger)
    {
        _inputOptions = inputOptionsMonitor.CurrentValue;

        _optionsChange = inputOptionsMonitor.OnChange(options =>
        {
            //check to secure from OnChange incorrect multiple times invoking
            if (options.Equals(_inputOptions))
                return;

            _inputOptions = options;
            Execute();
        });

        _fileWatcherFactory = fileWatcherFactory;
        _converterFactory = converterFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Execute();
        }
        catch (Exception e)
        {
            _logger.LogCrashError(e.Message);
            Dispose();
            throw;
        }

        return Task.CompletedTask;
    }

    private void Execute()
    {
        DisposeSubscriptions();
        
        foreach (var option in _inputOptions.FileInputList)
        {
            foreach (var path in option.FolderPaths)
            {
                var watcher = _fileWatcherFactory.GetWatcher(path, option.Extensions);
                    
                foreach (var extension in option.Extensions)
                {
                    if (!_converterFactory.TryGetConverter(extension, out var converter))
                        continue;
                
                    var subscription = watcher.Subscribe(converter);
                    _subscriptions.Add(subscription);
                }
            }
        }
        
        _fileWatcherFactory.ExtraWatchersDispose();
    }

    private void DisposeSubscriptions()
    {
        _subscriptions.ForEach(sub => sub.Dispose());
        _subscriptions.Clear();
    }

    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogStartApp();
        return base.StartAsync(cancellationToken);
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogStopApp();
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        DisposeSubscriptions();
        _fileWatcherFactory.Dispose();
        _converterFactory.Dispose();
        _optionsChange?.Dispose();
    }
}
