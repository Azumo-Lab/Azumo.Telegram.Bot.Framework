//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
