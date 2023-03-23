//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Telegram.Bot.Framework.Abstract.Actions;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.UpdateTypeActions.Actions
{
    /// <summary>
    /// 控制器的执行
    /// </summary>
    internal class ActionControllerInvoke : IAction
    {
        /// <summary>
        /// 执行控制器
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="NextHandle"></param>
        /// <returns></returns>
        public async Task Invoke(TelegramSession Context, ActionHandle NextHandle)
        {
            IControllerManager controllerManager = Context.UserService.GetService<IControllerManager>();
            IParamManager paramManger = Context.UserService.GetService<IParamManager>();

            TelegramController controller = controllerManager.GetController(paramManger.GetCommand(), out CommandInfo commandInfo);
            if (!controller.IsNull())
            {
                
            }
            controller ??= controllerManager.GetController(Context.Update.Message.Type, out commandInfo);
            controller ??= controllerManager.GetController(Context.Update.Type, out commandInfo);

            if (controller.IsNull())
                await NextHandle(Context);


            await controller.Invoke(Context, controllerManager.GetCommandInfo(paramManger.GetCommand()));

            await NextHandle(Context);
        }
    }
}
