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

namespace Azumo.PipelineMiddleware;

/// <summary>
/// 流水线阶段
/// </summary>
public enum PipelinePhase : int
{
    /// <summary>
    /// 前处理
    /// </summary>
    PreProcessing = 2,

    /// <summary>
    /// 一般处理
    /// </summary>
    GeneralProcessing = 4,

    /// <summary>
    /// 后处理
    /// </summary>
    PostProcessing = 8,
}
