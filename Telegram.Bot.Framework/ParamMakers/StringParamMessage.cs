using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ParamMakers
{
    internal class StringParamMessage : IParamMessage
    {
        public Task SendMessage(string Message, TelegramContext context)
        {
            throw new NotImplementedException();
        }
    }
}
