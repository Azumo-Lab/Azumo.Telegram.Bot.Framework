using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller
{
    public abstract class TelegramController
    {
        protected TelegramSession TelegramSession { get; private set; } = default!;

        public void Invoke(TelegramSession session)
        {
            TelegramSession = session;
        }

        protected virtual async Task Redirect(string commandName)
        {
            await Redirect(commandName, null!);
        }

        protected virtual async Task Redirect(string commandName, params object[] args)
        {
            IFrameworkInfo frameworkInfo = TelegramSession.UserService.GetRequiredService<IFrameworkInfo>();
            await frameworkInfo.CommandInvoke(TelegramSession.UserService, commandName, args);
        }

        protected virtual async Task Redirect(UpdateType updateType)
        {
            await Redirect(updateType, null!);
        }

        protected virtual async Task Redirect(UpdateType updateType, params object[] args)
        {
            IFrameworkInfo frameworkInfo = TelegramSession.UserService.GetRequiredService<IFrameworkInfo>();
            await frameworkInfo.CommandInvoke(TelegramSession.UserService, updateType, args);
        }
    }
}
