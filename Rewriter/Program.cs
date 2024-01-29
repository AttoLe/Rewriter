using FluentValidation;
using Rewriter;
using Rewriter.Configuration;
using Rewriter.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddFluentValidationOptions<FileConvertOptions>(FileConvertOptions.Key);
builder.Services.AddFluentValidationOptions<FileLoggerOptions>(FileLoggerOptions.Key);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Logging.ClearProviders();
builder.Logging.AddFileLog();

builder.Services.AddHostedService<Worker>();

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
