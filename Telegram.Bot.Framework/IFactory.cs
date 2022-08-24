using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework
{
    internal interface IFactory
    {
        public ITelegramRouteUserController NewTelegramRouteUserController();
    }
}
