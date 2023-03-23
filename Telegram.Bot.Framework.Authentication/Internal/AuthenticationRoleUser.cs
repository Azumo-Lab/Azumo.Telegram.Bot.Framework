using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Authentication.Interface;

namespace Telegram.Bot.Framework.Authentication.Internal
{
    internal class AuthenticationRoleUser : IAuthenticationRole
    {
        public string RoleName => "USER";

        public Task ChangeRole(TelegramSession session)
        {
            return Task.CompletedTask;   
        }
    }
}
