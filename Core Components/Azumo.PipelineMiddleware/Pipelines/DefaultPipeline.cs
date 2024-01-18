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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.PipelineMiddleware.Pipelines;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
internal class DefaultPipeline<TInput> : IPipeline<TInput>
{
    /// <summary>
    /// 
    /// </summary>
    private readonly MiddlewareDelegate<TInput> __Pipeline = (input) => Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    private readonly List<Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>> middlewares = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="middlewares"></param>
    /// <param name="pipelineController"></param>
    public DefaultPipeline(IEnumerable<IMiddleware<TInput>> middlewares, IPipelineController<TInput> pipelineController)
    {
        foreach (var middlewareOne in middlewares)
            this.middlewares.Add((handle) => input => middlewareOne.Execute(input, pipelineController));
        this.middlewares = this.middlewares.Reverse<Func<MiddlewareDelegate<TInput>, MiddlewareDelegate<TInput>>>().ToList();
        foreach (var middlewareTwo in this.middlewares)
            __Pipeline = middlewareTwo(__Pipeline);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task Invoke(TInput input) =>
        __Pipeline(input);
}
