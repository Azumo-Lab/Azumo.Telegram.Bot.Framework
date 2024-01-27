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
/// 默认的实现类，实现了接口 <see cref="IPipelineController{TInput}"/>
/// </summary>
/// <typeparam name="TInput"></typeparam>
[DebuggerDisplay("PipelineController")]
internal class DefaultPipelineController<TInput> : IPipelineController<TInput>
{
    /// <summary>
    /// 执行下一个操作的委托
    /// </summary>
    private MiddlewareDelegate<TInput>? middleware;

    /// <summary>
    /// 执行下一个操作的委托
    /// </summary>
    MiddlewareDelegate<TInput>? IPipelineController<TInput>.NextHandle
    {
        get => middleware;
        set => middleware = value;
    }

    /// <summary>
    /// 流水线Key和流水线的字典
    /// </summary>
    private readonly Dictionary<object, IPipeline<TInput>> __PipelineDic = [];

    /// <summary>
    /// 添加一条流水线
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="name"></param>
    public void AddPipeline(IPipeline<TInput> pipeline, object name)
    {
        if (__PipelineDic.TryAdd(name, pipeline))
            __PipelineDic[name] = pipeline;
    }

    /// <summary>
    /// 执行指定的流水线
    /// </summary>
    /// <remarks>
    /// 获取指定的Key的流水线实例，并执行流水线操作
    /// </remarks>
    /// <param name="name">流水线Key</param>
    /// <param name="input">要处理的数据</param>
    /// <returns>异步执行</returns>
    public Task Execute(object name, TInput input) =>
        __PipelineDic.TryGetValue(name, out var pipeline) ? pipeline.Invoke(input) : Task.CompletedTask;

    /// <summary>
    /// 执行下一步操作
    /// </summary>
    /// <remarks>
    /// 执行流水线的下一个操作，在中间件中，需要调用这个方法来进入下一个中间件处理流程
    /// </remarks>
    /// <param name="input">要处理的数据</param>
    /// <returns>异步执行</returns>
    public Task Next(TInput input) => 
        middleware?.Invoke(input, this) ?? Task.CompletedTask;

    /// <summary>
    /// 移除一个流水线
    /// </summary>
    /// <remarks>
    /// 从流水线列表中移除指定的流水线，如果没有指定Key的流水线，则不会报错
    /// </remarks>
    /// <param name="name">流水线Key</param>
    public void RemovePipeline(object name) =>
        __PipelineDic.Remove(name);

    /// <summary>
    /// 停止执行
    /// </summary>
    /// <param name="input">处理数据</param>
    /// <returns>异步执行</returns>
    public Task Stop(TInput input) => Task.CompletedTask;

    /// <summary>
    /// 获取流水线
    /// </summary>
    /// <remarks>
    /// 根据指定的Key获取流水线，如果指定的Key不能获取流水线的情况，则返回 <see cref="NullPipeline{TInput}.Instance"/> 
    /// </remarks>
    /// <param name="name">流水线对应的Key</param>
    /// <returns>流水线实例的引用</returns>
    public IPipeline<TInput> GetPipeline(object name) =>
        __PipelineDic.TryGetValue(name, out var pipeline) ?
        pipeline : NullPipeline<TInput>.Instance;
}
