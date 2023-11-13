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

        Action<string> action = (aa) => 
        { 
            AA(ref aa);
            BB(ref aa);
        };
        action(null!);
    }

    public static void AA(ref string str)
    {
        str = "STR";
        Console.WriteLine(str);
    }

    public static void BB(ref string str) 
    {
        str += "Hello";
        Console.WriteLine(str);
    }
}

internal static class TestEX
{
    public static ITelegramBotBuilder PrintHelloWorld(this ITelegramBotBuilder builder)
    {
        builder.AddTelegramPartCreator(new PrintHelloWorldClass());
        return builder;
    }

    private class PrintHelloWorldClass : IStartExec, ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services)
        {

        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            services.AddSingleton<IStartExec>(this);
        }

        public Task Exec(ITelegramBotClient bot, IServiceProvider serviceProvider)
        {
            ILogger Logger = serviceProvider.GetService<ILogger<PrintHelloWorldClass>>()!;
            Logger.LogInformation("Hello World !!!!");
            return Task.CompletedTask;
        }
    }
}