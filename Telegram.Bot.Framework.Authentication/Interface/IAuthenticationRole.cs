using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.Authentication.Interface
{
    public interface IAuthenticationRole
    {
        public string RoleName { get; }

        public Task ChangeRole(TelegramSession session);
    }
}
