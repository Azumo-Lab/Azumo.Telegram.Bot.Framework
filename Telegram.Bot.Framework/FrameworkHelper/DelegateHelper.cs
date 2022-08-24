using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Telegram.Bot.Framework.FrameworkHelper
{
    internal static class DelegateHelper
    {
        private static Dictionary<int, Type> ActionTypes = new Dictionary<int, Type>();
        private static Dictionary<int, Type> FuncTypes = new Dictionary<int, Type>();

        static DelegateHelper()
        {
            ActionTypes.Add(0, typeof(Action));
            ActionTypes.Add(1, typeof(Action<>));
            ActionTypes.Add(2, typeof(Action<,>));
            ActionTypes.Add(3, typeof(Action<,,>));
            ActionTypes.Add(4, typeof(Action<,,,>));
            ActionTypes.Add(5, typeof(Action<,,,,>));
            ActionTypes.Add(6, typeof(Action<,,,,,>));
            ActionTypes.Add(7, typeof(Action<,,,,,,>));
            ActionTypes.Add(8, typeof(Action<,,,,,,,>));
            ActionTypes.Add(9, typeof(Action<,,,,,,,,>));
            ActionTypes.Add(10, typeof(Action<,,,,,,,,,>));
            ActionTypes.Add(11, typeof(Action<,,,,,,,,,,>));
            ActionTypes.Add(12, typeof(Action<,,,,,,,,,,,>));
            ActionTypes.Add(13, typeof(Action<,,,,,,,,,,,,>));
            ActionTypes.Add(14, typeof(Action<,,,,,,,,,,,,,>));
            ActionTypes.Add(15, typeof(Action<,,,,,,,,,,,,,,>));
            ActionTypes.Add(16, typeof(Action<,,,,,,,,,,,,,,,>));

            FuncTypes.Add(0, typeof(Func<>));
            FuncTypes.Add(1, typeof(Func<,>));
            FuncTypes.Add(2, typeof(Func<,,>));
            FuncTypes.Add(3, typeof(Func<,,,>));
            FuncTypes.Add(4, typeof(Func<,,,,>));
            FuncTypes.Add(5, typeof(Func<,,,,,>));
            FuncTypes.Add(6, typeof(Func<,,,,,,>));
            FuncTypes.Add(7, typeof(Func<,,,,,,,>));
            FuncTypes.Add(8, typeof(Func<,,,,,,,,>));
            FuncTypes.Add(9, typeof(Func<,,,,,,,,,>));
            FuncTypes.Add(10, typeof(Func<,,,,,,,,,,>));
            FuncTypes.Add(11, typeof(Func<,,,,,,,,,,,>));
            FuncTypes.Add(12, typeof(Func<,,,,,,,,,,,,>));
            FuncTypes.Add(13, typeof(Func<,,,,,,,,,,,,,>));
            FuncTypes.Add(14, typeof(Func<,,,,,,,,,,,,,,>));
            FuncTypes.Add(15, typeof(Func<,,,,,,,,,,,,,,,>));
            FuncTypes.Add(16, typeof(Func<,,,,,,,,,,,,,,,,>));
        }

        public static Delegate CreateDelegate(MethodInfo methodInfo)
        {
            var T = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
            var returnType = methodInfo.ReturnType;

            Type delegateType = null;
            if (returnType.FullName == typeof(void).FullName)
            {
                delegateType = ActionTypes[T.Count];
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }
            else
            {
                delegateType = FuncTypes[T.Count];
                T.Add(returnType);
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }

            return Delegate.CreateDelegate(delegateType, methodInfo.ReflectedType, methodInfo);
        }

        public static Delegate CreateNewObj(Type type)
        {
            int Param = 100;
            ConstructorInfo constructorInfo = null;

            var constructors = type.GetConstructors().ToList();
            foreach (var item in constructors)
            {
                var count = item.GetParameters().Length;
                if (count < Param)
                {
                    constructorInfo = item;
                }
            }

            var paramInfos = constructorInfo.GetParameters();

            return default;
        }
    }
}
