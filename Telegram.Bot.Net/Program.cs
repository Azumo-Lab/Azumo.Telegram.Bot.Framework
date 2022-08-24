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

            var methodInfo = p.GetMethod("Config");
            var T = methodInfo.GetParameters().Select(x => x.ParameterType).ToList();

            // 创建委托类型
            var actionType = typeof(Action<>);
            // 泛型委托
            actionType = actionType.MakeGenericType(T.ToArray());

            TelegramServiceCollection telegramServices = new TelegramServiceCollection();

            //创建委托
            var method = Delegate.CreateDelegate(actionType, Activator.CreateInstance(p), methodInfo);
            //执行
            method.DynamicInvoke(telegramServices);



            int count = telegramServices.Count;
        }



        public void Config(IServiceCollection telegramServices)
        {
            telegramServices.Add(new TelegramServiceDescriptor());
        }
    }
}
