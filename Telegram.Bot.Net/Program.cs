using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.DependencyInjection;

namespace Telegram.Bot.Net
{
    class Program : ISetUp
    {
        static void Main(string[] args)
        {
            var bot = TelegramBotManger.Config()
                .SetToken("Token")
                .Proxy("localhost", 8080)
                .SetUp(new Program())
                .Build();

            bot.Start();
        }

        public void Config(IServiceCollection telegramServices)
        {
            
        }
    }
}
