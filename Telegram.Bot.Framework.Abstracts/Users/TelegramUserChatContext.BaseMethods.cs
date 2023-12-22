using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    public sealed partial class TelegramUserChatContext
    {
        private ChatId? __CacheUserChatID;

        private Chat? __CacheUserChat;
    }
}
