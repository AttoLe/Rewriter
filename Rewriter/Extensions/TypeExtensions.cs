using System.Reflection;
using Rewriter.Attributes;

namespace Rewriter.Extensions;

public static class TypeExtensions
{
    public static bool IsRightExtensionConverter(this Type type, string extension)
    {
        var attribute = type.GetCustomAttribute<ExtensionAttribute>();
        return attribute is { Extensions: { } extensions } &&
               extensions.Contains(Path.GetExtension(extension));
    }
}