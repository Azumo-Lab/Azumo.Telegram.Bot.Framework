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
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.InternalCore.Controller;

namespace Telegram.Bot.Framework.InternalCore.Install
{
    /// <summary>
    /// 
    /// </summary>
    internal class Factory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumCommandType"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IExecutor GetExecutorInstance(EnumCommandType enumCommandType, params object?[] objects)
        {
            IExecutor executor = enumCommandType switch
            {
                EnumCommandType.BotCommand => new BotCommandInvoker(
                                    (objects[0] as Func<IServiceProvider, object?[], object>)!,
                                    (objects[1] as List<IGetParam>)!,
                                    (objects[2] as Attribute[])!),
                EnumCommandType.Func => new FuncInvoker(
                                    (objects[0] as Delegate)!,
                                    (objects[1] as List<IGetParam>)!,
                                    (objects[2] as Attribute[])!),
                _ => throw new ArgumentException($"{nameof(enumCommandType)} 值非法"),
            };
            return executor;
        }
    }
}
