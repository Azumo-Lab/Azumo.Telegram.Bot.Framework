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
/// 流水线建造器接口
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IPipelineBuilder<TInput>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="middleware"></param>
    /// <param name="middlewareInsertionMode"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> Use(IMiddleware<TInput> middleware, MiddlewareInsertionMode middlewareInsertionMode = MiddlewareInsertionMode.EndOfPhase);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="invokeFilter"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> Use(IPipelineInvokeFilter<TInput> invokeFilter);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IPipelineBuilder<TInput> NewPipeline(object name);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IPipelineController<TInput> Build();
}
