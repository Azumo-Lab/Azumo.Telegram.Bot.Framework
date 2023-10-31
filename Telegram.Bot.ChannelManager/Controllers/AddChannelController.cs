using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts.Attributes;

namespace Telegram.Bot.ChannelManager.Controllers
{
    public class AddChannelController : TelegramController
    {
        public AddChannelController() { }

        [Authenticate("Admin")]
        [BotCommand("/AddChannel")]
        public async Task AddChannel()
        {
            await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, "Hello Admin");
        }
    }
}
