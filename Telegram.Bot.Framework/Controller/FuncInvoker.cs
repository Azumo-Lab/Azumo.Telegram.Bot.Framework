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
    /// 委托类型的执行
    /// </summary>
    internal class FuncInvoker : IExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="paramList"></param>
        /// <param name="attributes"></param>
        public FuncInvoker(Delegate func, List<IGetParam> paramList, Attribute[] attributes)
        {
            _Fun = func;
            Parameters = paramList;
            Attributes = attributes;
        }

        /// <summary>
        /// 
        /// </summary>
        public readonly Delegate _Fun;

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<IGetParam> Parameters { get; }

        /// <summary>
        /// 
        /// </summary>
        public Attribute[] Attributes { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Cache { get; } =
#if NET8_0_OR_GREATER
            [];
#else
            new Dictionary<string, object>();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramController"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<object?> Invoke(TelegramController telegramController, object?[] param)
        {
            object? result;
            return (result = _Fun.DynamicInvoke(param)) != null
                ? result is Task<IActionResult?> resultObj ? resultObj : Task.FromResult<IActionResult?>(result)
                : Task.FromResult<IActionResult?>(null);
        }
    }
}
