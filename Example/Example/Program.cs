using Telegram.Bot.Framework;

var telegramBot = TelegramBot.CreateBuilder()
    .UseClashDefaultProxy()
    .UseToken("<Token>")
    .Build();

await telegramBot.StartAsync(true);
