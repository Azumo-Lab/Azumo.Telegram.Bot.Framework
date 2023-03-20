using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Bots;
using Telegram.Bot.Framework.Utils;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string Token = args.GetArgs("-Token");
            string ProxyUri = args.GetArgs("-Proxy");

            ITelegramBot telegramBot = TelegramBotBuilder.Create()
                .AddToken(Token)
                .AddProxy(ProxyUri)
                .Build();

            Task botTask = telegramBot.BotStart();
            botTask.Wait();
        }
    }
}