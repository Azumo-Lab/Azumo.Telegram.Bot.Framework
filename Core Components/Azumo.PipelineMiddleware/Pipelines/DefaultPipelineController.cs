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
    /// 
    /// </summary>
    private MiddlewareDelegate<TInput>? middleware;

    /// <summary>
    /// 
    /// </summary>
    MiddlewareDelegate<TInput>? IPipelineController<TInput>.NextHandle
    {
        get => middleware;
        set => middleware = value;
    }

    /// <summary>
    /// 
    /// </summary>
    private readonly Dictionary<object, IPipeline<TInput>> __PipelineDic = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="name"></param>
    public void AddPipeline(IPipeline<TInput> pipeline, object name)
    {
        if (__PipelineDic.TryAdd(name, pipeline))
            __PipelineDic[name] = pipeline;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task Execute(object name, TInput input) =>
        __PipelineDic.TryGetValue(name, out var pipeline) ? pipeline.Invoke(input) : Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task Next(TInput input) => middleware?.Invoke(input, this) ?? Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void RemovePipeline(object name) =>
        __PipelineDic.Remove(name);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task Stop(TInput input) => Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IPipeline<TInput> GetPipeline(object name) =>
        __PipelineDic.TryGetValue(name, out var pipeline) ?
        pipeline : NullPipeline<TInput>.Instance;
}
