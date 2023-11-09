namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeForAttribute : Attribute
    {
        public Type Type { get; }
        public TypeForAttribute(Type type)
        {
            Type = type;
        }
    }
}
