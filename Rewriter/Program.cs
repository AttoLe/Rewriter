using Rewriter.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddValidatedConfiguration();
builder.AddWorkerComponent();
builder.AddFileLogging();

try
{
    var host = builder.Build();
    host.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
