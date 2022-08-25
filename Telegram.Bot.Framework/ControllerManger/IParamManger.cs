using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IParamManger
    {
        void ReadParam(TelegramContext context, IServiceProvider serviceProvider);

        bool IsReadParam(TelegramContext context);

        void Cancel(TelegramContext context);

        void SetCommand(string Command, TelegramContext context);

        object[] GetParam(TelegramContext context);
    }
}
