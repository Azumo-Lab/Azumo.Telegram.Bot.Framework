using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.InternalImpl.Bots;

internal class Program
{
    private static void Main(string[] args)
    {
        ITelegramBot telegramBot = TelegramBuilder.Create()
            .AddToken("5148150974:AAGDN_JERKYpQKuNbMuBqekTZotEOf6mVtI")
#if DEBUG
            .AddClashDefaultProxy()
#endif
            .Build();

        Task task = telegramBot.StartAsync();
        task.Wait();
    }
}