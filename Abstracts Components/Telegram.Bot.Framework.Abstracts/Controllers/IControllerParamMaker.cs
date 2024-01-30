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

using System.Reflection;

namespace Telegram.Bot.Framework.Abstracts.Controllers;

/// <summary>
/// 通过指定的参数类型，获取 <see cref="IControllerParam"/> 的实例
/// </summary>
internal interface IControllerParamMaker
{
    /// <summary>
    /// 获取实例
    /// </summary>
    /// <param name="parameterInfo"></param>
    /// <param name="controllerParamSender"></param>
    /// <returns></returns>
    public IControllerParam Make(ParameterInfo parameterInfo);
}
