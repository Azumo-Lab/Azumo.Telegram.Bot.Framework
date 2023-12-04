using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.Services;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Exec;
using Telegram.Bot.Framework.Bots;

namespace MyChannel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ITelegramBot telegramBot = TelegramBuilder.Create()
                .UseToken(x => x.GetSection("Token").Value)
                .UseClashDefaultProxy()
                .AddServices(serviceCollection =>
                {
                    serviceCollection.AddDbContext<MyDBContext>();

                    serviceCollection.AddSingleton<IStartExec, StartService>();
                    serviceCollection.AddSingleton<IExec, TimeingService>();
                })
                .AddConfiguration("C:\\Users\\ko--o\\Desktop\\test.json")
                .RegisterBotCommand()
                .AddSimpleConsole()
                .Build();

            Task botTask = telegramBot.StartAsync();
            botTask.Wait();
        }
    }
}
