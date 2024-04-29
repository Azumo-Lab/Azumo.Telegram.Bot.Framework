// See https://aka.ms/new-console-template for more information

using Telegram.Bot.Framework;

var telegramBot = TelegramBot.CreateBuilder()
    .UseClashDefaultProxy()
    .UseToken("<Token>")
    .Build();

await telegramBot.StartAsync(true);