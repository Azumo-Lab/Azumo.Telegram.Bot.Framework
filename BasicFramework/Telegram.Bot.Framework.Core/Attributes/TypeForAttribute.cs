namespace Telegram.Bot.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TypeForAttribute(Type type) : Attribute
{
    public Type ForType { get; } = type;
}
