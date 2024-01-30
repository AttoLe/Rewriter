using FluentValidation;
using Rewriter.Configuration;
using Rewriter.Extensions;
using Rewriter.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddFluentValidationOptions<FileInputOptions>(FileInputOptions.Key);
builder.Services.AddFluentValidationOptions<FileOutputOptions>(FileOutputOptions.Key);
builder.Services.AddFluentValidationOptions<FileLoggerOptions>(FileLoggerOptions.Key);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Logging.ClearProviders();
builder.Logging.AddFileLog();
builder.Services.AddConverterServices(builder.Configuration);

try
{
    var host = builder.Build();
    Console.WriteLine(host.Services.GetServices<BackgroundService>().Count());
    Console.WriteLine(host.Services.GetServices<IConverter>().Count());
   
    host.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}