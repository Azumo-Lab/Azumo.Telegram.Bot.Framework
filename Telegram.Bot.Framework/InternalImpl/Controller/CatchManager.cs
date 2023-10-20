using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Helpers;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(ICatchManager))]
    internal class CatchManager : ICatchManager
    {
        private readonly Dictionary<Type, Type> __TypeMap;
        public CatchManager()
        {
            __TypeMap = ReflectionHelper.FindTypeOf(typeof(ICatch)).ToDictionary(x =>
            {
                TypeForAttribute attribute = (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute));
                return attribute.Type;
            }, x => x);
        }
        public Type GetCatch(Type type)
        {
            return __TypeMap.TryGetValue(type, out Type result) ? result : null;
        }
    }
}
