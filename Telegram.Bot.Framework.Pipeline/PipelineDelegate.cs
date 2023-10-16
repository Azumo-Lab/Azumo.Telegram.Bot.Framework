﻿//  <Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    /// <summary>
    /// 流水线委托
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="processObj"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    public delegate Task<T> PipelineDelegate<T>(T processObj, IPipelineController<T> controller);
}
