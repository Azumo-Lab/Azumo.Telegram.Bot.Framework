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
    /// 
    /// </summary>
    internal class BotCommandInvoker : IExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="paramList"></param>
        /// <param name="attributes"></param>
        public BotCommandInvoker(Func<object, object?[], object?> func, List<IGetParam> paramList, Attribute[] attributes)
        {
            _func = func;
            Parameters = paramList;
            Attributes = attributes;
#if NET8_0_OR_GREATER
            Cache = [];
#else
            Cache = new Dictionary<string, object>();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<object, object?[], object?> _func;

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
        public Dictionary<string, object> Cache { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramController"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<object?> Invoke(TelegramController telegramController, object?[] param)
        {
            object? result;
            return (result = _func(telegramController, param)) != null
                ? result is Task<IActionResult?> resultObj ? resultObj : Task.FromResult<IActionResult?>(result)
                : Task.FromResult<IActionResult?>(null);
        }

    }
}
