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
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Framework.InternalImplementation.Sessions;

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
        public async Task Execute(ITelegramSession Session, IPipelineController PipelineController)
        {
            IControllerContextFactory controllerContextFactory = Session.UserService.GetService<IControllerContextFactory>();
            IControllerContext controllerContext = controllerContextFactory.CreateControllerContext(Session);
            if (controllerContext != null)
            {
                IParamMiddlewarePipeline paramMiddlewarePipeline = Session.UserService.GetService<IParamMiddlewarePipeline>();
                if (!await paramMiddlewarePipeline.Execute(Session, controllerContext))
                    return;

                IControllerFactory controllerFactory = Session.UserService.GetService<IControllerFactory>();
                TelegramController telegramController = controllerFactory.CreateController(controllerContext);

                telegramController.SetSession(Session);

                await controllerContext.Action(telegramController, paramMiddlewarePipeline.Param.ToArray());
            }

            await PipelineController.Next(Session);
        }
    }
}
