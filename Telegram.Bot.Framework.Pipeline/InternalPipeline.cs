//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    internal class InternalPipeline<T> : IPipeline<T>
    {
        /// <summary>
        /// 流水线的委托
        /// </summary>
        private readonly PipelineDelegate<T> __PipelineFuns = (t, c) => Task.FromResult(t);

        /// <summary>
        /// 
        /// </summary>
        private readonly IPipelineController<T> __Controller;

        /// <summary>
        /// 初始化，创建流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="procedures"></param>
        public InternalPipeline(IProcess<T>[] procedures, IPipelineController<T> pipelineController)
        {
            __Controller = pipelineController;

            List<Func<PipelineDelegate<T>, PipelineDelegate<T>>> procs = new();
            foreach (IProcess<T> proc in procedures)
                procs.Add(handle => (t, c) =>
                {
                    __Controller.SetNext(handle);
                    return proc.Execute(t, c);
                });

            foreach (Func<PipelineDelegate<T>, PipelineDelegate<T>> item in procs.Reverse<Func<PipelineDelegate<T>, PipelineDelegate<T>>>())
                __PipelineFuns = item(__PipelineFuns);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<T> Invoke(T obj)
        {
            return await __PipelineFuns(obj, __Controller);
        }
    }
}