namespace Rewriter;

public partial class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 
            LogHelloWorld(logger, "MEEE");
            await Task.Delay(1000, stoppingToken);
        }
    }
    
    [LoggerMessage(0, LogLevel.Information, "Writing hello world response to {dat}")]
    partial void LogHelloWorld(ILogger logger, string dat);
}