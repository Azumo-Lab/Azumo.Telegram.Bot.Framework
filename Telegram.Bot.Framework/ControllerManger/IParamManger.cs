using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IParamManger
    {
        object[] CreateParams(string Command);
    }
}
