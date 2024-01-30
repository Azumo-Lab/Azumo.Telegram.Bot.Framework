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
/// 流水线工厂类
/// </summary>
/// <remarks>
/// 使用这个静态工厂来创建相应的接口
/// </remarks>
public class PipelineFactory
{
    internal static Delegate DefaultValue = (Func<object>)(() => default!);

    /// <summary>
    /// 内部方法，创建流水线控制器实例
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <returns></returns>
    internal static IPipelineController<TInput, TResult> GetPipelineController<TInput, TResult>() =>
        new DefaultPipelineController<TInput, TResult>();

    /// <summary>
    /// 内部方法，创建流水线实例
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="middleware"></param>
    /// <param name="pipelineController"></param>
    /// <returns></returns>
    internal static IPipeline<TInput, TResult> GetPipeline<TInput, TResult>(MiddlewareDelegate<TInput, TResult> middleware, IPipelineController<TInput, TResult> pipelineController) =>
        new DefaultPipeline<TInput, TResult>(middleware, pipelineController);

    /// <summary>
    /// 创建流水线建造器
    /// </summary>
    /// <remarks>
    /// 创建一个流水线建造器，<typeparamref name="TInput"/> 是要进行处理的数据类型。
    /// </remarks>
    /// <typeparam name="TInput">要处理的数据类型</typeparam>
    /// <returns>返回流水线建造器实例</returns>
    public static IPipelineBuilder<TInput, TResult> GetPipelineBuilder<TInput, TResult>(Func<TResult> defaultValue)
    {
        DefaultValue = defaultValue;
        // 新生成一个默认的实现实例
        var builder = new DefaultPipelineBuilder<TInput, TResult>();

        // 使用一个默认的过滤器
        builder.Use(new ControllerPipelineInvokeFilter<TInput, TResult>());

        // 返回实例
        return builder;
    }
}
