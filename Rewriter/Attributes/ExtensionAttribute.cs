namespace Rewriter.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExtensionAttribute : Attribute
{
    public IEnumerable<string> Extensions { get; }

    public ExtensionAttribute(params string[] types)
    {
        Extensions = types;
    }
}
