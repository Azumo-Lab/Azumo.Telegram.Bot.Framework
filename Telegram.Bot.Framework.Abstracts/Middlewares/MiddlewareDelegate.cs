using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Session;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 委托
    /// </summary>
    /// <param name="Context"></param>
    /// <returns></returns>
    public delegate Task MiddlewareDelegate(ITelegramSession Session);
}
