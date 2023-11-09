using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    public interface IControllerParam
    {
        public Task SendMessage();

        public Task<object> CatchObjs(TGChat tGChat);
    }
}
