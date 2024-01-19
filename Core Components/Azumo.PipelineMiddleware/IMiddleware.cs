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
/// 流水线的处理中间件
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <typeparam name="TInput">传入的处理数据类型</typeparam>
public interface IMiddleware<TInput>
{
    /// <summary>
    /// 流水线执行阶段
    /// </summary>
    public PipelinePhase Phase { get; }

    /// <summary>
    /// 开始执行该阶段的处理
    /// </summary>
    /// <param name="input">传入的待处理数据</param>
    /// <param name="pipelineController">流水线控制器</param>
    /// <returns>异步执行</returns>
    public Task Execute(TInput input, IPipelineController<TInput> pipelineController);
}
