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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 控制器的执行
    /// </summary>
    internal class ActionControllerInvoke : IMiddleware
    {
        /// <summary>
        /// 执行控制器
        /// </summary>
        /// <param name="session"></param>
        /// <param name="NextHandle"></param>
        /// <returns></returns>
        public async Task Execute(TelegramSession session, MiddlewareHandle NextHandle)
        {
            IControllerManager controllerManager = session.UserService.GetService<IControllerManager>();
            IParamManager paramManger = session.UserService.GetService<IParamManager>();

            TelegramController controller;
            controller = controllerManager.GetController(paramManger.GetCommand(), out CommandInfo commandInfo);
            controller ??= controllerManager.GetController(session.Update.Message.Type, out commandInfo);
            controller ??= controllerManager.GetController(session.Update.Type, out commandInfo);

            if (controller.IsNull())
                await NextHandle(session);

            await controller.Invoke(session, commandInfo, paramManger.GetParam());

            await NextHandle(session);
        }
    }
}
