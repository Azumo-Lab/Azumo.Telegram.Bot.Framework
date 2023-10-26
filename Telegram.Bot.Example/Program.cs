using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.InternalImpl.Bots;

namespace Telegram.Bot.Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ITelegramBot bot = TelegramBuilder.Create()
                .AddToken("5148150974:AAGDN_JERKYpQKuNbMuBqekTZotEOf6mVtI")
                .AddClashDefaultProxy()
                .Build();

            bot.StartAsync().Wait();

            if (bot is IDisposable disposable)
                disposable.Dispose();
        }
    }

    
    public class TestController : TelegramController
    {
        private int count;

        [BotCommand]
        public async Task Test()
        {
            await Chat.BotClient.SendChatActionAsync(Chat.ChatId, Types.Enums.ChatAction.Typing);
            await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, $"第{count ++}次 Hello World !!");
        }

        [BotCommand("/Catch")]
        public async Task Test2(string str)
        {
            Logger.LogInformation($"发送的消息 : {str}");
            await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, $"You Say {str}");
        }
    }
}
