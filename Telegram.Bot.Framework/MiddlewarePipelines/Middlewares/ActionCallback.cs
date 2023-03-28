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
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Sessions;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 回调函数
    /// </summary>
    public class ActionCallback : IMiddleware
    {
        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="NextHandle">下一个处理流程</param>
        /// <returns></returns>
        public async Task Execute(ITelegramSession session, MiddlewareHandle NextHandle)
        {
            ICallBackManager callBackManager = session.UserService.GetService<ICallBackManager>();
            Action<ITelegramSession> callbackAction = callBackManager.GetCallBack(session.Update.CallbackQuery.Data);
            if (!callbackAction.IsNull())
            {
                callbackAction.Invoke(session);
                await session.BotClient.AnswerCallbackQueryAsync(session.Update.CallbackQuery.Id);
            }

            await NextHandle(session);
        }
    }
}
