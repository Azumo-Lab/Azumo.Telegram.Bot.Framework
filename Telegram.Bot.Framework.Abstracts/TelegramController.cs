using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        protected TGChat Chat { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Chat"></param>
        /// <param name="func"></param>
        /// <param name="controllerParamManager"></param>
        /// <returns></returns>
        internal async Task ControllerInvokeAsync(TGChat Chat, Func<TelegramController, object[], Task> func, IControllerParamManager controllerParamManager)
        {
            this.Chat = Chat;
            Logger = this.Chat.UserService.GetRequiredService<ILogger<TelegramController>>();
            try
            {
                if (func(this, controllerParamManager.GetParams() ?? Array.Empty<object>()) is Task task)
                    await task;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                controllerParamManager.Clear();
            }
        }
    }
}
