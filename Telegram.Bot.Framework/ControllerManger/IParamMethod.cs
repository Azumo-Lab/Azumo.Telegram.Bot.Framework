using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ControllerManger
{
    internal interface IParamMethod
    {
        Task<object> CreatePara(string MesageInfo, TelegramContext context);
    }
}
