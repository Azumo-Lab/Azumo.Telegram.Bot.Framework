using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IControllersManger
    {
        object GetController(string CommandName, IServiceProvider serviceProvider);
    }
}
