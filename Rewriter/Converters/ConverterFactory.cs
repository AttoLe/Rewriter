using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Rewriter.Extensions;

namespace Rewriter.Converters;

public class ConverterFactory(IServiceProvider serviceProvider, ILogger<ConverterFactory> logger) : IDisposable
{
    private readonly Dictionary<Type, AbstractConverter> _converters = [];

    public bool TryGetConverter(string extension,  [MaybeNullWhen(false)] out AbstractConverter converter)
    {
        if (!TryGetConverterType(extension, out var type))
        {
            logger.LogNoConverterError(extension);
            converter = null;
            return false;
        }

        if (_converters.TryGetValue(type, out var existedConverter))
        {
            converter = existedConverter;
            
            logger.LogAlreadyCreatedConverter(extension, type.Name);
            return true;
        }
        
        converter = CreateNewConverter(type);
        logger.LogNewConverterCreated(extension, type.Name);
        
        return true;
    }

    private AbstractConverter CreateNewConverter(Type type)
    {
        var newConverter = (AbstractConverter) ActivatorUtilities.CreateInstance(serviceProvider, type);
        _converters.Add(type, newConverter);

        return newConverter;
    }
    
    private static bool TryGetConverterType(string extension,  [MaybeNullWhen(false)] out Type type)
    {
        type = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass)
            .FirstOrDefault(t =>
                t.IsRightExtensionConverter(extension));
        
        return type is not null;
    }

    public void Dispose()
    {
        _converters.Clear();
    }
}