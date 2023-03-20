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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Controller.Internal
{
    /// <summary>
    /// 执行的基类
    /// </summary>
    internal abstract class InvokeBase
    {
        /// <summary>
        /// 执行一个指令
        /// </summary>
        /// <param name="commandInfo">指令信息</param>
        /// <param name="telegramController">控制器</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        protected async Task CommandInvoke(CommandInfo commandInfo, TelegramController telegramController, params object[] param)
        {
            Delegate @delegate = Delegate.CreateDelegate(commandInfo.ControllerType, telegramController, commandInfo.CommandMethod);
            Task? task;
            if (param.IsEmpty())
                task = @delegate.DynamicInvoke() as Task;
            else
                task = @delegate.DynamicInvoke(param) as Task;
            if (task != null)
                await task;
        }
    }
}
