using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.ControllerManger;

namespace Telegram.Bot.Framework
{
    internal class TelegramRouteUserController : ITelegramRouteUserController
    {
        public async Task Invoke(TelegramContext context, IServiceProvider serviceProvider)
        {
            string command = context.GetCommand();
            if (command != null)
            {
                IControllersManger controllersManger = serviceProvider.GetService<IControllersManger>();
                IDelegateManger delegateManger = serviceProvider.GetService<IDelegateManger>();

                Delegate action = delegateManger.CreateDelegate(command, controllersManger.GetController(command, serviceProvider));
            }
        }
    }
}
