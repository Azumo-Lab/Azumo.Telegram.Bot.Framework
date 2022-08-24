using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.FrameworkHelper;

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

            TestHelper.Test();

        }

        public void Config(IServiceCollection telegramServices)
        {
            throw new NotImplementedException();
        }
    }
}
