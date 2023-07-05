using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Authentication.Interface
{
    public interface IAuthenticationRole
    {
        public BotCommandScopeType Type { get; }

        public BotCommandScope GetBotCommandScope(ITelegramSession session, long? chatID, long? userID);

        public Task ChangeRole(ITelegramSession session);
    }
}
