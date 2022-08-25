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
            IParamManger paramManger = serviceProvider.GetService<IParamManger>();
            if (command != null)
            {
                if (paramManger.IsReadParam(context))
                    paramManger.Cancel(context);

                IControllersManger controllersManger = serviceProvider.GetService<IControllersManger>();

                if (controllersManger.HasCommand(command))
                {
                    TelegramController controller = (TelegramController)controllersManger.GetController(command, serviceProvider);
                    await controller.Invoke(context, serviceProvider, command);
                }
                else
                {
                    
                }
            }
            else
            {
                if (paramManger.IsReadParam(context))
                {

                }
                else
                {

                }
            }
        }
    }
}
