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

using System;
using System.Collections.Generic;
using Telegram.Bot.Framework.InternalCore.PipelineMiddleware;

namespace Telegram.Bot.Framework.Core.PipelineMiddleware
{
    /// <summary>
    /// 管道工厂
    /// </summary>
    public class PipelineFactory
    {
        /// <summary>
        /// 创建一个管道构建器
        /// </summary>
        /// <typeparam name="TInput">传入类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <returns>管道构造器 <see cref="IPipelineBuilder{TInput, TResult}"/> 的实例</returns>
        public static IPipelineBuilder<TInput, TResult> GetPipelineBuilder<TInput, TResult>(Func<TResult> defVal) =>
            new PipelineBuilder<TInput, TResult>(defVal);

        /// <summary>
        /// 内部方法，用于创建一个管道
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="middleware"></param>
        /// <returns></returns>
        internal static IPipeline<TInput, TResult> GetPipeline<TInput, TResult>(PipelineMiddlewareDelegate<TInput, TResult> middleware) =>
            new Pipeline<TInput, TResult>(middleware);

        /// <summary>
        /// 内部方法，用于创建一个管道控制器
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dics"></param>
        /// <returns></returns>
        internal static IPipelineController<TInput, TResult> GetPipelineController<TInput, TResult>(Dictionary<object, IPipeline<TInput, TResult>> dics) =>
            new PipelineController<TInput, TResult>(dics);
    }
}
