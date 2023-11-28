using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace MyChannel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ITelegramBot telegramBot = TelegramBuilder.Create()
                .AddBasic();
                
        }
    }
}
