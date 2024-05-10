using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Authentication
{
    internal interface IBanList
    {
        public HashSet<ChatId> ChatIds { get; }

        public bool IsBanned(ChatId? chatId);
    }
}
