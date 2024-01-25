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
/// <remarks>
/// 表示流水线的执行阶段，在当前版本的流水线模型下，流水线阶段被分为了以下的几个阶段。
/// </remarks>
public enum PipelinePhase : int
{
    /// <summary>
    /// 前处理
    /// </summary>
    /// <remarks>
    /// 前处理阶段是进行数据的预处理阶段，例如判断数据是否合规，数据是否需要进行处理等。
    /// </remarks>
    PreProcessing = 2,

    /// <summary>
    /// 一般处理
    /// </summary>
    /// <remarks>
    /// 在这个阶段，就是开始进行数据处理的阶段。
    /// </remarks>
    GeneralProcessing = 4,

    /// <summary>
    /// 后处理
    /// </summary>
    /// <remarks>
    /// 后处理阶段，数据在上一个阶段处理完毕，就进入后处理阶段，在这个阶段中，数据会被检测是否处理失败。
    /// 数据是否需要修补等操作。
    /// </remarks>
    PostProcessing = 8,
}
