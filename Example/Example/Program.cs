using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework;

var telegramBot = TelegramBot.CreateBuilder()
    .AddServiceAction(x =>
    {
        x.AddMemoryCache();
    })
    .UseController()
    .UseClashDefaultProxy()
    .UseToken("5148150974:AAGDN_JERKYpQKuNbMuBqekTZotEOf6mVtI")
    .AddSimpleConsole()
    .Build();

await telegramBot.StartAsync(true);
