using Azumo.ShellGenerate;
using Azumo.ShellGenerate.Interfaces;
using Azumo.ShellGenerate.Tokens;
using Azumo.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.ChannelManager;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Bots;
using Telegram.Bot.Framework.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        ITelegramBot telegramBot = TelegramBuilder.Create()
            .UseToken(Secret.Token)
#if DEBUG
            .UseClashDefaultProxy()
#endif
            .RegisterBotCommand()
            .Build();

        Task task = telegramBot.StartAsync();
        task.Wait();
    }
}