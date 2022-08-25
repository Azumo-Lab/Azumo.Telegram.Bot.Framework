using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;
using Telegram.Bot.Framework.TelegramMessage;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Net.Example
{
    public class Commands : TelegramController
    {
        private readonly IServiceProvider serviceProvider;
        public Commands(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Command("Test")]
        public async Task Test()
        {
            await SendTextMessage("Hello");
            await SendTextMessage("hello", new List<InlineKeyboardButton> { InlineKeyboardButton.WithUrl("百度一下", "") });
        }
    }
}
