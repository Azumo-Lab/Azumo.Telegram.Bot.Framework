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
            ICommandManager commandManager = TelegramSession.UserService.GetRequiredService<ICommandManager>();
            await commandManager.CommandInvoke(commandName, args);
        }

        protected virtual async Task Redirect(UpdateType updateType)
        {
            await Redirect(updateType, null!);
        }

        protected virtual async Task Redirect(UpdateType updateType, params object[] args)
        {
            ICommandManager commandManager = TelegramSession.UserService.GetRequiredService<ICommandManager>();
            await commandManager.CommandInvoke(updateType, args);
        }
    }
}
