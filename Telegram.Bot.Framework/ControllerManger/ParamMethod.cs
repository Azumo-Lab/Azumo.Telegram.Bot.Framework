using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal class ParamMethod : IParamMethod
    {
        public async Task<object> CreatePara(string MessageInfo, TelegramContext context)
        {
            var bot = context.BotClient;
            await bot.SendTextMessageAsync(context.ChatID, MessageInfo);
        }
    }
}
