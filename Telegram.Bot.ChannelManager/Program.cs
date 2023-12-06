using Telegram.Bot.ChannelManager;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Bots;

internal class Program
{
    private static void Main(string[] args)
    {
        var telegramBot = TelegramBuilder.Create()
            .UseToken(Secret.Token)
#if DEBUG
            .UseClashDefaultProxy()
#endif
            .RegisterBotCommand()
            .Build();

        var task = telegramBot.StartAsync();
        task.Wait();
    }
}