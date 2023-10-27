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

using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;
using BotCommand = Telegram.Bot.Framework.Reflections.BotCommand;

namespace Telegram.Bot.Framework.Abstracts.Controller
{
    /// <summary>
    /// 控制器执行接口
    /// </summary>
    internal interface IControllerInvoker
    {
        /// <summary>
        /// 执行 <see cref="TelegramController"/> 控制器
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tGChat"></param>
        /// <param name="controllerParamManager"></param>
        /// <returns></returns>
        Task InvokeAsync(BotCommand command, TGChat tGChat, IControllerParamManager controllerParamManager);

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <remarks>
        /// 从本次请求中获取这次的指令
        /// </remarks>
        /// <param name="update">单次请求</param>
        /// <returns>取得的指令</returns>
        BotCommand GetCommand(Update update);
    }
}
