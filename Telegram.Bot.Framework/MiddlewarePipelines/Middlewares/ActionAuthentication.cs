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
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Authentication.Interface;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using static System.Collections.Specialized.BitVector32;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 认证相关的类
    /// </summary>
    internal class ActionAuthentication : IMiddleware
    {
        /// <summary>
        /// 执行认证
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="PipelineController"></param>
        /// <returns></returns>
        public async Task Execute(IChat Session, IPipelineController PipelineController)
        {
            IEnumerable<IAuthentication> authentications = Session.UserService.GetServices<IAuthentication>();

            if (authentications.IsEmpty())
            {
                await PipelineController.Next(Session);
                return;
            }

            foreach (IAuthentication authentication in authentications)
                if (!await authentication.AuthUser(Session))
                {
                    await authentication.ErrorMessage(Session);
                    return;
                }

            await PipelineController.Next(Session);
        }
    }
}
