using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Bots;

namespace MyChannel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ITelegramBot telegramBot = TelegramBuilder.Create()
                .UseToken("")
                .UseClashDefaultProxy()
                .AddServices(serviceCollection =>
                {
                    serviceCollection.AddDbContext<MyDBContext>();
                })
                .Build();

            Task botTask = telegramBot.StartAsync();
            botTask.Wait();
        }
    }
}
