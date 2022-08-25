using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ParamMakers
{
    internal class StringParamMessage : IParamMessage
    {
        public async Task SendMessage(string Message, TelegramContext context)
        {
            await context.BotClient.SendTextMessageAsync(context.ChatID, Message);
        }
    }
}
