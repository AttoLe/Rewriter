namespace Rewriter.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ExtensionAttribute(params string[] types) : Attribute
{
}
