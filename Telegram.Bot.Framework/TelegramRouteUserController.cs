using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework
{
    internal class TelegramRouteUserController : ITelegramRouteUserController
    {
        public async Task Invoke(TelegramContext context, IServiceProvider serviceProvider)
        {
            string command = context.GetCommand();
            if (command != null)
            {
                IParamCheck check = (IParamCheck)serviceProvider.GetService(typeof(IParamCheck));
            }
        }
    }
}
