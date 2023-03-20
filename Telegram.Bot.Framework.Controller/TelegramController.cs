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
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class TelegramController
    {
        protected TelegramSession Session { get; private set; } = default!;

        public void Invoke(TelegramSession session)
        {
            Session = session;
        }

        #region (Redirect)跳转至其他的方法

        protected virtual async Task Redirect(string commandName)
        {
            await Redirect(commandName, null!);
        }

        protected virtual async Task Redirect(string commandName, params object[] args)
        {
            ICommandInvoker commandManager = Session.UserService.GetRequiredService<ICommandInvoker>();
            await commandManager.CommandInvoke(commandName, args);
        }

        protected virtual async Task Redirect(MessageType messageType)
        {
            await Redirect(messageType, null!);
        }

        protected virtual async Task Redirect(MessageType messageType, params object[] args)
        {
            ICommandInvoker commandInvoker = Session.UserService.GetRequiredService<ICommandInvoker>();
            await commandInvoker.CommandInvoke(messageType, args);
        }

        protected virtual async Task Redirect(UpdateType updateType)
        {
            await Redirect(updateType, null!);
        }

        protected virtual async Task Redirect(UpdateType updateType, params object[] args)
        {
            IUpdateTypeInvoker commandManager = Session.UserService.GetRequiredService<IUpdateTypeInvoker>();
            await commandManager.CommandInvoke(updateType, args);
        }

        #endregion

    }
}
