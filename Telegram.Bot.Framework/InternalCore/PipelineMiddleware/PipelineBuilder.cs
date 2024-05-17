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
using System.Linq;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.PipelineMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal class PipelineBuilder<TInput, TResult> : IPipelineBuilder<TInput, TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defVal"></param>
        public PipelineBuilder(Func<TResult> defVal) =>
            this.defVal = defVal;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<TResult> defVal;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<object, List<IMiddleware<TInput, TResult>>> pipelines =
#if NET8_0_OR_GREATER
            [];
#else
            new Dictionary<object, List<IMiddleware<TInput, TResult>>>();
#endif

        /// <summary>
        /// 
        /// </summary>
        private readonly List<IMiddleware<TInput, TResult>> middleware =
#if NET8_0_OR_GREATER
            [];
#else
            new List<IMiddleware<TInput, TResult>>();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPipelineController<TInput, TResult> Build()
        {
            var pipelineDic = new Dictionary<object, IPipeline<TInput, TResult>>();
            if (pipelines.Count == 0)
                _ = CreatePipeline(Guid.NewGuid().ToString());
            foreach (var item in pipelines)
            {
                PipelineMiddlewareDelegate<TInput, TResult> handleResult = input => defVal();
                var list = new List<Func<PipelineMiddlewareDelegate<TInput, TResult>, PipelineMiddlewareDelegate<TInput, TResult>>>();
                foreach (var middleware in item.Value)
                    list.Add(handle => input => middleware.Execute(input, handle));
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
}
