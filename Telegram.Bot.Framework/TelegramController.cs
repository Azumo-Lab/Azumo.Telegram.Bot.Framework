using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TelegramController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Task<IActionResult> InvokeBotCommand()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ControllerResult> InvokeController()
        {
            try
            {
                var actionResult = await InvokeBotCommand();
                await actionResult.ExecuteAsync(null!);

                return ControllerResult.Success;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        public virtual Task OnActionExecutionAsync()
        {

        }

        public virtual void OnActionExecuting() { }

        public virtual void OnActionExecuted() { }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ControllerResult
    {
        /// <summary>
        /// 
        /// </summary>
        Success,

        /// <summary>
        /// 
        /// </summary>
        Unauthorized,

        /// <summary>
        /// 
        /// </summary>
        Forbidden,

        /// <summary>
        /// 
        /// </summary>
        WaitParamter,
    }
}
