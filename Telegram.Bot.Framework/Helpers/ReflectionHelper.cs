using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Helpers
{
    internal static class ReflectionHelper
    {
        public static IReadOnlyList<Type> AllTypes { get; }

        static ReflectionHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }
        public static List<Type> FindTypeOf(this object obj)
        {
            Type type;
            if (obj is Type newType)
                type = newType;
            else 
                type = obj.GetType();

            return AllTypes.Where(type.IsAssignableFrom).Where(x => !x.IsInterface && !x.IsAbstract).ToList();
        }
    }
}
