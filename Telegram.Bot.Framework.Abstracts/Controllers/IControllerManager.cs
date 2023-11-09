using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal interface IControllerManager
    {
        BotCommand GetCommand(TGChat tGChat);
    }
}
