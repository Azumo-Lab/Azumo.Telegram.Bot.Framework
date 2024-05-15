using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.TelegramMessage;

namespace Telegram.Bot.Framework.Core.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ActionResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        protected ActionResult()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task ExecuteResultAsync(TelegramActionContext context)
        {
            
        }
    }
}
