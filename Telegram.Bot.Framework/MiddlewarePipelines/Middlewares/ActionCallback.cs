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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;

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
        /// <param name="Session">Context</param>
        /// <param name="PipelineController">下一个处理流程</param>
        /// <returns></returns>
        public async Task Execute(IChat Session, IPipelineController PipelineController)
        {
            ICallBackManager callBackManager = Session.UserService.GetService<ICallBackManager>();
            Action<ITelegramSession> callbackAction = callBackManager.GetCallBack(Session.Update.CallbackQuery.Data);
            if (!callbackAction.IsNull())
            {
                callbackAction.Invoke(Session);
                await Session.BotClient.AnswerCallbackQueryAsync(Session.Update.CallbackQuery.Id);
            }

            await PipelineController.Next(Session);
        }
    }
}
