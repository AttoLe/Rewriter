using Microsoft.Extensions.Options;
using Rewriter.Configuration;
using Rewriter.FileWatchers;
using Rewriter.Workers;

namespace Rewriter;

public class Worker<TConverter> : BackgroundService where TConverter : IConverter
{
    private FileInputOptions _inputOptions;
    private ILogger<TConverter> _logger;
    private FileWatcherProvider _fileWatcherProvider;
    private TConverter _converter;
    public Worker(
        IOptionsMonitor<FileInputOptions> fileInputOptionsMonitor,
        ILogger<TConverter> logger,
        FileWatcherProvider fileWatcherProvider,
        TConverter converter
    )
    {
        _logger = logger;
        _fileWatcherProvider = fileWatcherProvider;
        _converter = converter;
        _inputOptions = fileInputOptionsMonitor.CurrentValue;
        fileInputOptionsMonitor.OnChange(options => _inputOptions = options);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var inputFolderPath in _inputOptions.FolderPaths)
            {
                //logger
                _fileWatcherProvider.AddWatcher(inputFolderPath, _inputOptions.Extensions, _converter.ConvertFile);
            }
        }

        return Task.CompletedTask;
    }
}