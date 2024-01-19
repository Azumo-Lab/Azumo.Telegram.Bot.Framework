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

using Azumo.PipelineMiddleware.Pipelines;

namespace Azumo.PipelineMiddleware;

/// <summary>
/// 
/// </summary>
public class PipelineFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <returns></returns>
    internal static IPipelineController<TInput> GetPipelineController<TInput>() =>
        new DefaultPipelineController<TInput>();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="middleware"></param>
    /// <param name="pipelineController"></param>
    /// <returns></returns>
    internal static IPipeline<TInput> GetPipeline<TInput>(MiddlewareDelegate<TInput> middleware, IPipelineController<TInput> pipelineController) =>
        new DefaultPipeline<TInput>(middleware, pipelineController);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <returns></returns>
    public static IPipelineBuilder<TInput> GetPipelineBuilder<TInput>()
    {
        var builder = new DefaultPipelineBuilder<TInput>();
        builder.Use(new ControllerPipelineInvokeFilter<TInput>());
        return builder;
    }
}
