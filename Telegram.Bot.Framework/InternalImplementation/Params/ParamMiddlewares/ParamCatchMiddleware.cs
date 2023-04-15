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
        public async Task<bool> Execute(ITelegramSession Session, IParamManager paramManager, IControllerContext controllerContext, ParamMiddlewareDelegate Next)
        {
            if (paramManager.MakeMode)
            {
                IParamMaker paramMaker = paramManager.GetMaker(Session.UserService, controllerContext.ParamModels[paramManager.ParamIndex].ParamMaker ?? controllerContext.ParamModels[paramManager.ParamIndex].ParamType);
                if (!await paramMaker.ParamCheck(Session))
                    return false;

                paramManager.AddParam(await paramMaker.GetParam(Session));
                paramManager.MakeMode = !paramManager.MakeMode;
                paramManager.ParamIndex++;

                if (paramManager.ParamIndex < controllerContext.ParamModels.Count)
                    return await Execute(Session, paramManager, controllerContext, Next);
            }
            else
            {
                IParamMessage paramMessage = (IParamMessage)ActivatorUtilities.CreateInstance(Session.UserService, controllerContext.ParamModels[paramManager.ParamIndex].ParamMsg, Array.Empty<object>());
                await paramMessage.SendMessage(Session, controllerContext.ParamModels[paramManager.ParamIndex].ParamAttr.Message ?? "请输入参数：");
                paramManager.MakeMode = !paramManager.MakeMode;

                return false;
            }

            return await Next(Session, paramManager, controllerContext);
        }
    }
}
