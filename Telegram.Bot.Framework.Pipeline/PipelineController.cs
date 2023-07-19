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

using System.Text;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineController<T> : IPipelineController<T>
    {
        private readonly Dictionary<object, IPipeline<T>> __Pipelines = new();

        private PipelineDelegate<T> __Next;
        private IPipeline<T> __NowPipeline;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="pipeline"></param>
        public void AddPipeline<PipelineNameType>(PipelineNameType pipelineName, IPipeline<T> pipeline) where PipelineNameType : notnull
        {
            __Pipelines.Add(pipelineName, pipeline);
        }

        public string GetInvokePath()
        {
            // TODO: 记录流水线的执行路径，用于调试
            StringBuilder stringBuilder = new();
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<T> Next(T t)
        {
            if (__NowPipeline != null)
                return await __NowPipeline.Invoke(t);
            else if (__Next != null)
                return await __Next(t, this);

            throw new NullReferenceException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Task<T> Stop(T t)
        {
            return Task.FromResult(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<T> SwitchTo<PipelineNameType>(PipelineNameType pipelineName, T t) where PipelineNameType : notnull
        {
            if (__Pipelines.TryGetValue(pipelineName, out IPipeline<T> val))
                __NowPipeline = val;
            return await Next(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineDelegate"></param>
        void IPipelineController<T>.SetNext(PipelineDelegate<T> pipelineDelegate)
        {
            __Next = pipelineDelegate;
            __NowPipeline = null!;
        }
    }
}
