using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IDelegateManger
    {
        Delegate CreateDelegate(string Command, object controller);
    }
}
