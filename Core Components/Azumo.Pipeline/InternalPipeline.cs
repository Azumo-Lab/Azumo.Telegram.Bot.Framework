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

using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Azumo.Pipeline
{
    /// <summary>
    /// 一个内部实现的流水线
    /// </summary>
    internal class InternalPipeline<T> : IPipeline<T>
    {
        /// <summary>
        /// 流水线的委托
        /// </summary>
        private readonly PipelineDelegate<T> __PipelineFuns = (t, c) => Task.FromResult(t);

        /// <summary>
        /// 流水线控制器
        /// </summary>
        private readonly IPipelineController<T> __Controller;

        /// <summary>
        /// 过滤器集合
        /// </summary>
        private readonly List<IPipelineFilter> pipelineFilters = PipelineFactory.ServiceProvider.GetServices<IPipelineFilter>().ToList();

        /// <summary>
        /// 初始化，创建流水线
        /// </summary>
        /// <param name="procedures">流水线的处理工序</param>
        /// <param name="pipelineController">控制器</param>
        public InternalPipeline(IProcessAsync<T>[] procedures, IPipelineController<T> pipelineController)
        {
            __Controller = pipelineController;

            List<Func<PipelineDelegate<T>, PipelineDelegate<T>>> procs = [];
            foreach (IProcessAsync<T> proc in procedures)
                procs.Add(handle => (model, controller) =>
                {
                    // TODO: 这部分逻辑比较复杂，稍后进行详细的解释
                    foreach (IPipelineFilter filter in pipelineFilters)
                    {
                        (model, bool next) = filter.Execute(model, controller, proc, handle);
                        if (!next)
                            return Task.FromResult(model);
                    }
                    return proc.ExecuteAsync(model, controller);
                });

            foreach (Func<PipelineDelegate<T>, PipelineDelegate<T>> item in procs.Reverse<Func<PipelineDelegate<T>, PipelineDelegate<T>>>())
                __PipelineFuns = item(__PipelineFuns);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="obj">处理数据</param>
        /// <returns>流水线处理后数据</returns>
        public async Task<T> Invoke(T obj)
        {
            return await __PipelineFuns(obj, __Controller);
        }
    }
}