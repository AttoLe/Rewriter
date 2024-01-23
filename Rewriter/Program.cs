using Rewriter;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Logging.ClearProviders();

var host = builder.Build();
host.Run();