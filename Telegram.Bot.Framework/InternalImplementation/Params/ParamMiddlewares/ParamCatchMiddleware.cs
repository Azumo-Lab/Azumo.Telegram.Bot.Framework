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
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.InternalImplementation.Params.ParamMiddlewares
{
    /// <summary>
    /// 
    /// </summary>
    internal class ParamCatchMiddleware : IParamMiddleware
    {
        private int __Index;

        private bool __MakerMode;

        public async Task<bool> Execute(ITelegramSession Session, IParamManager paramManager, IControllerContext controllerContext, ParamMiddlewareDelegate Next)
        {
            if (__MakerMode)
            {
                IParamMaker paramMaker = ActivatorUtilities.CreateInstance<IParamMaker>(Session.UserService, controllerContext.ParamModels[__Index].ParamMaker, Array.Empty<object>());
                if (!await paramMaker.ParamCheck(Session))
                    return false;

                paramManager.AddParam(await paramMaker.GetParam(Session));
                __MakerMode = !__MakerMode;
                __Index++;

                if (__Index < controllerContext.ParamModels.Count)
                    return await Execute(Session, paramManager, controllerContext, Next);
            }
            else
            {
                IParamMessage paramMessage = ActivatorUtilities.CreateInstance<IParamMessage>(Session.UserService, controllerContext.ParamModels[__Index].ParamMsg, Array.Empty<object>());
                await paramMessage.SendMessage(controllerContext.ParamModels[__Index].ParamAttr.Message ?? "请输入参数：");
                __MakerMode = !__MakerMode;

                return false;
            }

            return await Next(Session, paramManager, controllerContext);
        }
    }
}
