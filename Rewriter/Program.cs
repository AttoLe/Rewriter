using FluentValidation;
using Rewriter;
using Rewriter.Configuration;
using Rewriter.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Logging.ClearProviders();
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddFluentValidationOptions<FileConvertConfig>(FileConvertConfig.Key);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

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
