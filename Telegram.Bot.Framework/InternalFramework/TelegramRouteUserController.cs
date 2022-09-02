//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework.InternalFramework
{
    internal class TelegramRouteUserController : ITelegramRouteUserController
    {
        private readonly TelegramContext context;
        private readonly IServiceProvider serviceProvider;

        public TelegramRouteUserController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            context = this.serviceProvider.GetService<TelegramContext>();
        }

        public async Task Invoke()
        {
            string command = context.GetCommand();

            IParamManger paramManger = serviceProvider.GetService<IParamManger>();
            IControllersManger controllersManger = serviceProvider.GetService<IControllersManger>();

            if (command != null)
            {
                if (paramManger.IsReadParam())
                    paramManger.Cancel();

                if (!controllersManger.HasCommand(command))
                    command = "/";

                paramManger.SetCommand();
                if (!await paramManger.StartReadParam())
                    return;

                TelegramController controller = (TelegramController)controllersManger.GetController(command);
                await controller.Invoke(context, serviceProvider, command);
            }
            else
            {
                if (paramManger.IsReadParam())
                {
                    if (!await paramManger.StartReadParam())
                    {
                        return;
                    }
                    command = paramManger.GetCommand();
                    TelegramController controller = (TelegramController)controllersManger.GetController(command);
                    await controller.Invoke(context, serviceProvider, command);
                }
                else
                {

                }
            }
        }
    }
}
