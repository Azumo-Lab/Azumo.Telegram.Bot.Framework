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
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <param name="controller"></param>
internal class DefaultPipelineBuilder<TInput> : IPipelineBuilder<TInput>
{
    /// <summary>
    /// 
    /// </summary>
    private object key = null!;

    /// <summary>
    /// 
    /// </summary>
    private readonly Dictionary<object, List<IMiddleware<TInput>>> __Func = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly IPipelineController<TInput> __Controller = PipelineFactory.GetPipelineController<TInput>();

    /// <summary>
    /// 
    /// </summary>
    private readonly List<IPipelineInvokeFilter<TInput>> __InvokeFilters = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly List<IPipelineFilter<TInput>> __Filters = [];

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IPipelineController<TInput> Build()
    {
        var func = __Func.ToDictionary(x => x.Key, y => y.Value.Select<IMiddleware<TInput>, Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>>(z => (handle) => (input, controller) =>
            {
                foreach (var item in __InvokeFilters)
                    if (!item.Filter(handle, z, input, controller))
                        return Task.CompletedTask;
                return z.Execute(input, controller);
            }));
        foreach (var fun in func)
        {
            MiddlewareDelegate<TInput> middlewareDelegate = (input, controller) => Task.CompletedTask;
            foreach (var item in fun.Value.Reverse<Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>>())
                middlewareDelegate = item(middlewareDelegate);

            __Controller.AddPipeline(PipelineFactory.GetPipeline(middlewareDelegate, __Controller), fun.Key);
        }
        return __Controller;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> NewPipeline(object name)
    {
        key = name;
        _ = __Func.TryAdd(name, []);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="middleware"></param>
    /// <param name="middlewareInsertionMode"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> Use(IMiddleware<TInput> middleware, MiddlewareInsertionMode middlewareInsertionMode = MiddlewareInsertionMode.EndOfPhase)
    {
        if (key == null)
            throw new Exception($"Call {NewPipeline} Method");

        static void Add(List<IMiddleware<TInput>> list, IMiddleware<TInput> middleware, MiddlewareInsertionMode middlewareInsertionMode)
        {
            switch (middlewareInsertionMode)
            {
                case MiddlewareInsertionMode.EndOfPhase:
                    list.Add(middleware);
                    break;
                case MiddlewareInsertionMode.StartOfPhase:
                    list.Insert(0, middleware);
                    break;
                default:
                    break;
            }
        }

        var result =
            __Func[key]
            .GroupBy(x => x.Phase)
            .ToDictionary(x => x.Key, x => x.ToList());

        Add(result[middleware.Phase], middleware, middlewareInsertionMode);

        __Func[key].Add(middleware);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="invokeFilter"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> Use(IPipelineInvokeFilter<TInput> invokeFilter)
    {
        __InvokeFilters.Add(invokeFilter);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> Use(IPipelineFilter<TInput> filter)
    {
        __Filters.Add(filter);
        return this;
    }
}
