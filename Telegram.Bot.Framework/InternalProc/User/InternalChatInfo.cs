using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    internal class InternalChatInfo : IChatInfo
    {
        public InternalChatInfo(Chat Chat, Types.User ChatUser)
        {
            this.Chat = Chat;
            this.ChatUser = ChatUser;
        }

        public Types.User ChatUser { get; set; }

        public Types.User SendUser { get; set; }

        public Chat Chat { get; set; }

        public bool IsBan { get; set; }
    }
}
