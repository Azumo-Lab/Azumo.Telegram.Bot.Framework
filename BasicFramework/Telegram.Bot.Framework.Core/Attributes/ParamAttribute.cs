namespace Telegram.Bot.Framework.Core.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ParamAttribute : Attribute
{
    public string Message { get; set; } = string.Empty;

    public Type? IGetParmType { get; set; }
}
