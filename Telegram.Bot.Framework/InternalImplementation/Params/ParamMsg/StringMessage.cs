using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.ExtensionMethods;

namespace Telegram.Bot.Framework.InternalImplementation.Params.ParamMsg
{
    [TypeFor(typeof(string))]
    internal class StringMessage : IParamMessage
    {
        public async Task SendMessage(ITelegramSession telegramSession, string Message)
        {
            await telegramSession.SendTextMessageAsync(Message);
        }
    }
}
