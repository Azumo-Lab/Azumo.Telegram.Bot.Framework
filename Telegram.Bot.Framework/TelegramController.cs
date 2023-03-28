using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class TelegramController
    {
        protected TelegramSession Session { get; private set; } = default!;

        /// <summary>
        /// 控制器执行
        /// </summary>
        /// <param name="session">用户的Session</param>
        /// <param name="commandInfo">指令信息</param>
        /// <param name="param">执行的参数信息</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task Invoke(TelegramSession session, CommandInfo commandInfo, params object[] param)
        {
            if (session.IsNull())
                throw new ArgumentNullException(nameof(session));

            Session = session;
            ICommandInvoker commandInvoker = Session.UserService.GetRequiredService<ICommandInvoker>();
            if (commandInfo.IsNull())
                return;
            await commandInvoker.CommandInvoke(commandInfo, this, param);
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
