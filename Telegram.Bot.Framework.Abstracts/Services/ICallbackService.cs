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
using Telegram.Bot.Framework.Abstracts.User;

namespace Telegram.Bot.Framework.Abstracts.Services
{
    /// <summary>
    /// 用于回调的服务
    /// </summary>
    public interface ICallbackService : IDisposable
    {
        /// <summary>
        /// 创建一个回调函数
        /// </summary>
        /// <param name="callbackfunc"></param>
        /// <returns></returns>
        public string CreateCallback(Func<IChat, Task> callbackfunc);

        /// <summary>
        /// 执行回调
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        public Task InvokeCallback(IChat chat);

        /// <summary>
        /// 移除一个回调函数
        /// </summary>
        /// <param name="callbackData"></param>
        public void RemoveCallback(string callbackData);
    }
}
