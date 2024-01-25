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

using System.Diagnostics;

namespace Azumo.PipelineMiddleware.Pipelines;

/// <summary>
/// 一个空的，不进行任何处理的流水线实现
/// </summary>
/// <remarks>
/// 这个实现了 <see cref="IPipeline{TInput}"/> 接口，是一个不执行任何操作的空流水线实现
/// </remarks>
/// <typeparam name="TInput">处理的数据类型</typeparam>
[DebuggerDisplay("NullPipeline")]
internal class NullPipeline<TInput> : IPipeline<TInput>
{
    /// <summary>
    /// 空执行
    /// </summary>
    /// <remarks>
    /// 方法不进行任何处理，返回 <see cref="Task.CompletedTask"/>
    /// </remarks>
    /// <param name="input">传入数据</param>
    /// <returns>异步任务</returns>
    public Task Invoke(TInput input) => Task.CompletedTask;

    /// <summary>
    /// 静态的实例
    /// </summary>
    public static IPipeline<TInput> Instance { get; } = new NullPipeline<TInput>();
}
