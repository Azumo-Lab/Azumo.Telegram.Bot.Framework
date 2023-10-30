using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IMessageManager))]
    internal class MessageManager : IMessageManager
    {
        private readonly Dictionary<Type, Type> __TypeMap;
        public MessageManager()
        {
            __TypeMap = ReflectionHelper.FindTypeOf(typeof(IMessage)).ToDictionary(x =>
            {
                TypeForAttribute attribute = (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute));
                return attribute.Type;
            }, x => x);
        }
        public Type GetMessage(Type type)
        {
            return __TypeMap.TryGetValue(type, out Type messageType) ? messageType : default;
        }
    }
}
