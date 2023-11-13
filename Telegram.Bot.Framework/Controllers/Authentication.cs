using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers
{
    public class Authentication : IControllerFilter
    {
        public virtual async Task<bool> Execute(TGChat tGChat, BotCommand botCommand)
        {
            if (botCommand == null)
                return false;
            return true;
        }
    }
}
