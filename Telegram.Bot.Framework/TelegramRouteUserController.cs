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
            IControllersManger controllersManger = serviceProvider.GetService<IControllersManger>();

            if (command != null)
            {
                if (paramManger.IsReadParam(context))
                    paramManger.Cancel(context);

                if (!controllersManger.HasCommand(command))
                    command = "/";

                paramManger.SetCommand(command, context);
                paramManger.StartReadParam(context, serviceProvider);

                TelegramController controller = (TelegramController)controllersManger.GetController(command, serviceProvider);
                await controller.Invoke(context, serviceProvider, command);
            }
            else
            {
                if (paramManger.IsReadParam(context))
                {
                    paramManger.StartReadParam(context, serviceProvider);
                }
                else
                {

                }
            }
        }
    }
}
