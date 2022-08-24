using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework
{
    internal interface ITelegramRouteUserController
    {
        Task Invoke(TelegramContext context, IServiceProvider serviceProvider);
    }
}
