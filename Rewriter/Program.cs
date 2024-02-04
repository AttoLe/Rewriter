using FluentValidation;
using Rewriter.Configuration;
using Rewriter.Converters;
using Rewriter.Extensions;
using Rewriter.FileWatchers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddFluentValidationOptions<FileInputOptions>(FileInputOptions.Key);
builder.Services.AddFluentValidationOptions<FileOutputOptions>(FileOutputOptions.Key);
builder.Services.AddFluentValidationOptions<FileLoggerOptions>(FileLoggerOptions.Key);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddSingleton<FileWatcherFactory>();
builder.Services.AddSingleton<ConverterFactory>();

builder.Logging.ClearProviders();
builder.Logging.AddFileLog();

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