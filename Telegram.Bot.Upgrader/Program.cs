using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Upgrader;

ITelegramBot tgbot = TelegramBotManger.Create()
    .SetToken("<your token>")
    .SetProxy("http://127.0.0.1", 7890)
    .AddConfig<StartUp>()
    .Build();

Task bot = tgbot.BotStart();
bot.Wait();
