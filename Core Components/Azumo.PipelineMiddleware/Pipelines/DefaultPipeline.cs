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

namespace Azumo.PipelineMiddleware.Pipelines;

/// <summary>
/// 默认的流水线实现类
/// </summary>
/// <typeparam name="TInput">流水线处理类型</typeparam>
/// <param name="__Middleware">中间件的委托</param>
/// <param name="pipelineController">流水线控制器的应用</param>
internal class DefaultPipeline<TInput>(MiddlewareDelegate<TInput> __Middleware, IPipelineController<TInput> pipelineController) : IPipeline<TInput>
{
    /// <summary>
    /// 流水线委托
    /// </summary>
    private readonly MiddlewareDelegate<TInput> __Middleware = __Middleware;

    /// <summary>
    /// 流水线控制器
    /// </summary>
    private readonly IPipelineController<TInput> __PipelineController = pipelineController;

    /// <summary>
    /// 开始执行本条流水线
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="input">要处理的数据</param>
    /// <returns>异步任务</returns>
    public Task Invoke(TInput input) =>
        __Middleware(input, __PipelineController);
}
