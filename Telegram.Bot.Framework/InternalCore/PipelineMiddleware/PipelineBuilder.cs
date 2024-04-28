//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Core.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.PipelineMiddleware;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
/// <param name="defVal"></param>
internal class PipelineBuilder<TInput, TResult>(Func<TResult> defVal) : IPipelineBuilder<TInput, TResult>
{
    /// <summary>
    /// 
    /// </summary>
    private readonly Dictionary<object, List<IMiddleware<TInput, TResult>>> pipelines = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly List<IMiddleware<TInput, TResult>> middleware = [];

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IPipelineController<TInput, TResult> Build()
    {
        Dictionary<object, IPipeline<TInput, TResult>> pipelineDic = [];
        if (pipelines.Count == 0)
            _ = CreatePipeline(Guid.NewGuid().ToString());
        foreach (var item in pipelines)
        {
            PipelineMiddlewareDelegate<TInput, TResult> handleResult = input => defVal();
            List<Func<PipelineMiddlewareDelegate<TInput, TResult>, PipelineMiddlewareDelegate<TInput, TResult>>> list = [];
            foreach (var middleware in item.Value)
                list.Add(handle => input => middleware.Invoke(input, handle));
            foreach (var handle in list.Reverse<Func<PipelineMiddlewareDelegate<TInput, TResult>, PipelineMiddlewareDelegate<TInput, TResult>>>())
                handleResult = handle(handleResult);

            pipelineDic.Add(item.Key, PipelineFactory.GetPipeline(handleResult));
        }
        return PipelineFactory.GetPipelineController(pipelineDic);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IPipelineBuilder<TInput, TResult> CreatePipeline(object key)
    {
        if (middleware.Count == 0)
            throw new Exception();
        pipelines.Add(key, new List<IMiddleware<TInput, TResult>>(middleware));
        middleware.Clear();
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="middleware"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput, TResult> Use(IMiddleware<TInput, TResult> middleware)
    {
        this.middleware.Add(middleware);
        return this;
    }
}
