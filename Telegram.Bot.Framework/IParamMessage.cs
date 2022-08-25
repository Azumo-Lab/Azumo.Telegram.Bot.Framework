using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework
{
    public interface IParamMessage
    {
        Task SendMessage(string Message, TelegramContext context);
    }
}
