//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Results;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// 执行器
    /// </summary>
    internal interface IExecutor
    {
        /// <summary>
        /// 可以使用的 Attribute
        /// </summary>
        public Attribute[] Attributes { get; }

        /// <summary>
        /// 方法执行的参数
        /// </summary>
        public IReadOnlyList<IGetParam> Parameters { get; }

        /// <summary>
        /// 缓存
        /// </summary>
        public Dictionary<string, object> Cache { get; }

        /// <summary>
        /// 方法的执行
        /// </summary>
        /// <param name="telegramController">控制器实例</param>
        /// <param name="param">参数列表</param>
        /// <returns></returns>
        public Task<object?> Invoke(TelegramController telegramController, object?[] param);
    }
}
