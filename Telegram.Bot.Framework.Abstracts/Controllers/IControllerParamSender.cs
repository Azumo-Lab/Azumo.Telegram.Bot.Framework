using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    public interface IControllerParamSender
    {
        Task Send(ITelegramBotClient botClient, ChatId chatId);
    }
}
