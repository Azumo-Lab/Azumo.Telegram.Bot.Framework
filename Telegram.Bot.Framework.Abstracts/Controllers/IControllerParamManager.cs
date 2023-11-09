using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    internal interface IControllerParamManager
    {
        public List<IControllerParam> ControllerParams { get; set; }

        public object[] GetParams();

        public Task<ResultEnum> NextParam(TGChat tGChat);

        public BotCommand GetBotCommand();

        public void SetBotCommand(BotCommand botCommand);

        public void Clear();
    }
}
