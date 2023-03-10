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
using Telegram.Bot.Framework.InternalFramework.Abstract;

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
        public async Task Invoke(TelegramContext Context, ActionHandle NextHandle)
        {
            IControllerManager controllerManager = Context.UserScope.GetService<IControllerManager>();
            IParamManager paramManger = Context.UserScope.GetService<IParamManager>();

            TelegramController controller = controllerManager.CreateController(paramManger.GetCommand());
            controller ??= controllerManager.CreateController(Context.Update.Message.Type);
            await controller.Invoke(Context, controllerManager.GetCommandInfo(paramManger.GetCommand()));

            await NextHandle(Context);
        }
    }
}
