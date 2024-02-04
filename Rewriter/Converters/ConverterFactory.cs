using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Options;
using Rewriter.Attributes;
using Rewriter.Configuration;

namespace Rewriter.Converters;

public class ConverterFactory(IServiceProvider serviceProvider) : IDisposable
{
    private readonly Dictionary<Type, AbstractConverter> _converters = [];

    public bool TryGetConverter(string extension,  [MaybeNullWhen(false)] out AbstractConverter converter)
    {
        if (!TryGetConverterType(extension, out var type))
        {
            //logger
            converter = null;
            return false;
        }

        if (_converters.TryGetValue(type, out var existedConverter))
        {
            converter = existedConverter;
            return true;
        }
        
        converter = CreateNewConverter(type);
        return true;
    }

    private AbstractConverter CreateNewConverter(Type type)
    {
        var logger = serviceProvider.GetRequiredService(typeof(ILogger<>).MakeGenericType(type));
        var options = serviceProvider.GetRequiredService<IOptionsMonitor<FileOutputOptions>>();
        var newConverter = (AbstractConverter) ActivatorUtilities.CreateInstance(serviceProvider, type, options, logger);
        _converters.Add(type, newConverter);

        return newConverter;
    }
    
    private static bool TryGetConverterType(string extension,  [MaybeNullWhen(false)] out Type type)
    {
        type = TryFindConverterType(extension);
        return type is not null;
    }

    private static Type? TryFindConverterType(string extension)
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass)
            .FirstOrDefault(t =>
                t.GetCustomAttribute<ExtensionAttribute>() is { Extensions: { } extensions } &&
                extensions.Contains(extension)
            );
    }

    public void Dispose()
    {
        _converters.Clear();
    }
}