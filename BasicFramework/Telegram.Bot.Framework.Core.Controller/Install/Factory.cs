//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;

namespace Telegram.Bot.Framework.Core.Controller.Install;
internal class Factory
{
    public static IExecutor GetExecutorInstance(EnumCommandType enumCommandType, params object[] objects)
    {
        IExecutor executor = enumCommandType switch
        {
            EnumCommandType.BotCommand => new BotCommandInvoker(
                                (objects[0] as ObjectFactory)!,
                                (objects[1] as Func<object, object[], object>)!,
                                (objects[2] as List<IGetParam>)!,
                                (objects[3] as Attribute[])!),
            EnumCommandType.Func => new FuncInvoker(
                                (objects[0] as Delegate)!,
                                (objects[1] as List<IGetParam>)!,
                                (objects[2] as Attribute[])!),
            _ => throw new ArgumentException($"{nameof(enumCommandType)} 值非法"),
        };
        return executor;
    }
}
