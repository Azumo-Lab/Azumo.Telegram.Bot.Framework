using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class TelegramController
    {
        protected ITelegramSession Session { get; private set; } = default!;

        internal void SetSession(ITelegramSession session) {  Session = session; }
    }
}
