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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.UpdateTypeActions.Actions
{
    /// <summary>
    /// 回调函数
    /// </summary>
    public class ActionCallback : IAction
    {
        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="NextHandle">下一个处理流程</param>
        /// <returns></returns>
        public async Task Invoke(TelegramSession session, ActionHandle NextHandle)
        {
            ICallBackManager callBackManager = Context.UserScope.GetService<ICallBackManager>();
            Action<TelegramContext> callbackAction = callBackManager.GetCallBack(Context.Update.CallbackQuery.Data);
            if (!callbackAction.IsNull())
            {
                await Context.BotClient.AnswerCallbackQueryAsync(Context.Update.CallbackQuery.Id);
                callbackAction.Invoke(session);
            }

            await NextHandle(session);
        }
    }
}
