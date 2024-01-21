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
/// 流水线执行时的过滤器
/// </summary>
/// <remarks>
/// 在流水线执行时的过滤器，这个过滤器的执行时期是每个中间件执行之前进行执行。
/// </remarks>
/// <typeparam name="TInput">流水线处理的数据类型</typeparam>
public interface IPipelineInvokeFilter<TInput>
{
    /// <summary>
    /// 在中间件执行之前，执行过滤器操作
    /// </summary>
    /// <param name="handle">下一步执行的中间件委托</param>
    /// <param name="middleware">下一步执行的中间件</param>
    /// <param name="input">传入的处理参数</param>
    /// <param name="pipelineController">流水线控制器</param>
    /// <returns>是否继续执行下一步操作</returns>
    public bool Filter(Delegate handle, IMiddleware<TInput> middleware, TInput input, IPipelineController<TInput> pipelineController);
}
