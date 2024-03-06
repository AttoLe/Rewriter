using Rewriter.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.SetCurrentDirectory()
    .AddWindowServiceInjection()
    .AddValidatedConfiguration()
    .AddWorkerComponent()
    .AddFileLogging();

try
{
    var host = builder.Build();
    host.Run();
}
catch (Exception e)
{
    Environment.Exit(1);
}
