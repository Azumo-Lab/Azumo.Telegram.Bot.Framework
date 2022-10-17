//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Managers;
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework.InternalFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class ActionControllerInvoke : IAction
    {
        public int Sort => 300;

        public async Task Invoke(TelegramContext context, ActionHandle NextHandle)
        {
            IControllersManager controllersManger = context.UserScope.GetService<IControllersManager>();
            // 获取参数管理
            IParamManager paramManger = context.UserScope.GetService<IParamManager>();

            TelegramController controller = (TelegramController)controllersManger.GetController(paramManger.GetCommand());
            if (controller != null)
                await controller.Invoke(context, paramManger.GetCommand());
            else
            {
                CommandInfos infos = controllersManger.GetMessageTypeCommandInfos(context.Update.Message.Type, Array.Empty<Type>().ToList());
                if(infos != null)
                {
                    TelegramController telegramController = (TelegramController)context.UserScope.GetService(infos.Controller);
                    await telegramController.Invoke(context, infos);
                }
            }

            await NextHandle(context);
        }
    }
}
