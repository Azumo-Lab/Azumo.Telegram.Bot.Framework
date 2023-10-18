using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.Abstracts.Controller.Filters
{
    public interface IControllerInvokeFilter
    {
        public Task InvokeAfter(TelegramController controller, TGChat chat);

        public Task InvokeBefore(TelegramController controller, TGChat chat);

        public Task InvokeWhenError(TelegramController controller, TGChat chat, Exception ex);
    }
}
