using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.DependencyInjection;

namespace Telegram.Bot.Net
{
    class Program : ISetUp
    {
        static void Main(string[] args)
        {
            //var bot = TelegramBotManger.Config()
            //    .SetToken("Token")
            //    .Proxy("localhost", 8080)
            //    .SetUp(new Program())
            //    .Build();

            //bot.Start();
            

            var p = typeof(Program);

            var methodInfo = p.GetMethod(nameof(Test));

            object telegramServices = new TelegramServiceCollection();

            //创建委托
            var method = GetDelegate(methodInfo);
            //执行
            method.DynamicInvoke("TestAA", "TestBB");
        }

        private static Delegate GetDelegate(MethodInfo methodInfo)
        {
            var T = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();
            var returnType = methodInfo.ReturnType;

            Type delegateType = null;
            if (returnType.FullName == typeof(void).FullName)
            {
                delegateType = typeof(Action<>);
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }
            else
            {
                switch (T.Count)
                {
                    case 0:
                        delegateType = typeof(Func<>);
                        break;
                    case 1:
                        delegateType = typeof(Func<,>);
                        break;
                    case 2:
                        delegateType = typeof(Func<,,>);
                        break;
                    case 3:
                        delegateType = typeof(Func<,,,>);
                        break;
                    case 4:
                        delegateType = typeof(Func<,,,,>);
                        break;
                    case 5:
                        delegateType = typeof(Func<,,,,,>);
                        break;
                    case 6:
                        delegateType = typeof(Func<,,,,,,>);
                        break;
                    case 7:
                        delegateType = typeof(Func<,,,,,,,>);
                        break;
                    case 8:
                        delegateType = typeof(Func<,,,,,,,,>);
                        break;
                    default:
                        break;
                }
                T.Add(returnType);
                delegateType = delegateType.MakeGenericType(T.ToArray());
            }

            return Delegate.CreateDelegate(delegateType, Activator.CreateInstance(methodInfo.ReflectedType), methodInfo);
        }



        public void Config(IServiceCollection telegramServices)
        {
            telegramServices.Add(new TelegramServiceDescriptor());
        }

        public string Test(string AA, string BB)
        {
            Console.WriteLine(AA);
            Console.WriteLine(BB);

            return "CC";
        }
    }
}
