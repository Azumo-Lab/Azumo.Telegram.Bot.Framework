using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller
{
    internal interface ICallBackManager
    {
        public InlineKeyboardButton CreateCallBackButton(ButtonResult buttonResult);

        public IExecutor? GetCallBack(TelegramRequest telegramRequest);
    }
}
