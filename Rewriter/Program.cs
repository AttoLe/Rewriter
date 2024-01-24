using FluentValidation;
using Rewriter;
using Rewriter.Configuration;
using Rewriter.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Logging.ClearProviders();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddOptions<FileConvertConfig>()
    .BindConfiguration(FileConvertConfig.Key)
    .ValidateFluentValidation().ValidateOnStart();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var host = builder.Build();
host.Run();