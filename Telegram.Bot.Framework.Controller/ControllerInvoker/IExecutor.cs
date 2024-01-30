using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controller.ControllerInvoker;
/// <summary>
/// 
/// </summary>
internal interface IExecutor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public Task Invoke(IServiceProvider serviceProvider, TelegramUserChatContext context);
}
