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
/// 默认的流水线建造器
/// </summary>
/// <remarks>
/// 一个默认的流水线创造器，实现了 <see cref="IPipelineBuilder{TInput}"/> 接口。
/// </remarks>
/// <typeparam name="TInput">要处理的数据类型</typeparam>
internal class DefaultPipelineBuilder<TInput> : IPipelineBuilder<TInput>
{
    /// <summary>
    /// 当前设定的流水线的Key
    /// </summary>
    private object __CurrentPipelineKey = null!;

    /// <summary>
    /// 当前的流水线中间件字典
    /// </summary>
    private readonly Dictionary<object, List<IMiddleware<TInput>>> __CurrentPipelineDic = [];

    /// <summary>
    /// 流水线控制器引用
    /// </summary>
    private readonly IPipelineController<TInput> __Controller = PipelineFactory.GetPipelineController<TInput>();

    /// <summary>
    /// 流水线执行时的过滤器
    /// </summary>
    private readonly List<IPipelineInvokeFilter<TInput>> __InvokeFilters = [];

    /// <summary>
    /// 流水线开头和结尾的过滤器(前处理和后处理过滤器)
    /// </summary>
    private readonly List<IPipelineFilter<TInput>> __Filters = [];

    /// <summary>
    /// 将中间件实例转换成委托
    /// </summary>
    /// <remarks>
    /// 将中间件实例转换成委托后，创建流水线，并设置流水线控制器要进行控制的流水线。
    /// </remarks>
    /// <returns>流水线控制器引用</returns>
    public IPipelineController<TInput> Build()
    {
        // 创建委托列表
        var func = __CurrentPipelineDic.ToDictionary(x => x.Key, y => y.Value.Select<IMiddleware<TInput>, Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>>(z => (handle) => (input, controller) =>
            {
                foreach (var item in __InvokeFilters)
                    if (!item.Filter(handle, z, input, controller))
                        return Task.CompletedTask;
                return z.Execute(input, controller);
            }));
        
        // 将委托列表转换为单个委托
        foreach (var fun in func)
        {
            MiddlewareDelegate<TInput> middlewareDelegate = (input, controller) => Task.CompletedTask;
            foreach (var item in fun.Value.Reverse<Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>>())
                middlewareDelegate = item(middlewareDelegate);

            __Controller.AddPipeline(PipelineFactory.GetPipeline(middlewareDelegate, __Controller), fun.Key);
        }

        // 返回控制器的实例引用
        return __Controller;
    }

    /// <summary>
    /// 创建一个新的流水线
    /// </summary>
    /// <remarks>
    /// 当流水线Key： <paramref name="pipelineKey"/> 的值是 <see cref="null"/> 的时候，
    /// 则会抛出异常 <see cref="ArgumentNullException"/>
    /// </remarks>
    /// <param name="pipelineKey">流水线Key</param>
    /// <exception cref="ArgumentNullException"><paramref name="pipelineKey"/>的值为NULL</exception>
    /// <returns>添加了流水线Key的本实例引用</returns>
    public IPipelineBuilder<TInput> NewPipeline(object pipelineKey)
    {
        __CurrentPipelineKey = pipelineKey;

        ArgumentNullException.ThrowIfNull(__CurrentPipelineKey, nameof(pipelineKey));

        // 尝试添加到流水线字典中
        _ = __CurrentPipelineDic.TryAdd(__CurrentPipelineKey, []);
        return this;
    }

    /// <summary>
    /// 使用指定中间件
    /// </summary>
    /// <remarks>
    /// 使用指定的中间件，中间件要实现 <see cref="IMiddleware{TInput}"/> 接口，同时推荐实现 <see cref="IMiddlewareName"/> 接口，
    /// 为中间件添加名称。
    /// <br></br>
    /// <br></br>
    /// 需要注意的是，执行本方法之前，需要先执行 <see cref="NewPipeline(object)"/> 方法，创建一个新的流水线，否则将会抛出异常 <see cref="Exception"/>
    /// <br></br>
    /// <br></br>
    /// 如果参数 <paramref name="middlewareInsertionMode"/> 的类型或值，不是 <see cref="MiddlewareInsertionMode"/> 的范围的话，则会抛出 <see cref="ArgumentException"/> 异常
    /// </remarks>
    /// <param name="middleware">中间件的实例</param>
    /// <param name="middlewareInsertionMode">中间件插入模式</param>
    /// <returns>添加中间件的本实例引用</returns>
    /// <exception cref="Exception">如果没有执行 <see cref="NewPipeline(object)"/> 方法，则会抛出异常</exception>
    /// <exception cref="ArgumentException"><see cref="MiddlewareInsertionMode"/> 的类型错误</exception>
    public IPipelineBuilder<TInput> Use(IMiddleware<TInput> middleware, MiddlewareInsertionMode middlewareInsertionMode = MiddlewareInsertionMode.EndOfPhase)
    {
        if (__CurrentPipelineKey == null)
            throw new Exception($"Call {NewPipeline} Method");

        if (!__CurrentPipelineDic.TryGetValue(__CurrentPipelineKey, out var list))
            list = [];

        var phase = middleware.Phase;

        var phaseList = list.GroupBy(x => x.Phase).ToDictionary(x => x.Key, x => x.ToList());
        if (phaseList.TryAdd(phase, [middleware]))
            goto LIST;

        switch (middlewareInsertionMode)
        {
            case MiddlewareInsertionMode.EndOfPhase:
                phaseList[phase].Add(middleware);
                break;
            case MiddlewareInsertionMode.StartOfPhase:
                phaseList[phase].Insert(0, middleware);
                break;
            default:
                throw new ArgumentException($"{nameof(middlewareInsertionMode)} Type Error");
        }

    LIST:
        list.Clear();
        list.AddRange(phaseList.OrderBy(x => x.Key).SelectMany(x => x.Value).ToList());

        return this;
    }

    /// <summary>
    /// 使用流水线执行过滤器
    /// </summary>
    /// <param name="invokeFilter">流水线执行时过滤器实例引用</param>
    /// <returns>返回当前实例的引用</returns>
    public IPipelineBuilder<TInput> Use(IPipelineInvokeFilter<TInput> invokeFilter)
    {
        __InvokeFilters.Add(invokeFilter);
        return this;
    }

    /// <summary>
    /// 使用流水线过滤器
    /// </summary>
    /// <param name="filter">流水线过滤器的实例引用</param>
    /// <returns>返回当前实例的引用</returns>
    public IPipelineBuilder<TInput> Use(IPipelineFilter<TInput> filter)
    {
        __Filters.Add(filter);
        return this;
    }
}
