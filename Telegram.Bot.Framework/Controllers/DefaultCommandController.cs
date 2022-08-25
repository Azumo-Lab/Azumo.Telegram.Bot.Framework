using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.Controllers
{
    public class DefaultCommandController : TelegramController
    {
        [Command("")]
        public async Task DefaultCommand()
        {
            await SendTextMessage("非常抱歉，本Bot不支持这个指令");
        }
    }
}
