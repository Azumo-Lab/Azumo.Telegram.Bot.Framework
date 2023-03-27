using System.ComponentModel;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Bots;
using Telegram.Bot.Framework.Controller;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ITelegramBot telegramBot = TelegramBotBuilder.Create()
                .AddToken("hello")
#if DEBUG
                // 正式服务器上，不需要使用代理
                .AddProxy("http://127.0.0.1:7890")
#endif
                .UseController()
                .Build();

            Task botTask = telegramBot.BotStart();
            botTask.Wait();
        }
    }
}