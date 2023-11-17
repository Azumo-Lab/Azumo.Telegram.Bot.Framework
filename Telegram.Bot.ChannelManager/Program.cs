using Azumo.ShellGenerate;
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
        //        ITelegramBot telegramBot = TelegramBuilder.Create()
        //            .UseToken(Secret.Token)
        //#if DEBUG
        //            .UseClashDefaultProxy()
        //#endif
        //            .RegisterBotCommand()
        //            .PrintHelloWorld()
        //            .Build();

        //        Task task = telegramBot.StartAsync();
        //        task.Wait();

        new TestShellGen().Invoke(null!);
    }

    private class TestShellGen : Shell
    {
        protected override List<TokenBase> GenerateToken()
        {
            Var hello = Token<Var>().SetName("Hello").SetValue("Hello World");
            return
            [
                hello | Token<Echo>().Param(hello),
            ];
        }
    }

    private class TestOne
    {
        public string TestOneWWW { get; set; } = "WWE";
    }
}