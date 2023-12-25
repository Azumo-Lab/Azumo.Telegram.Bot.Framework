using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    public interface IUser
    {
        public User User { get; set; }

        public Chat UserChat { get; }

        public ChatId UserChatID { get; }

        public ISession Session { get; }
    }
}
