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
    /// 内部实现的流水线控制器
    /// </summary>
    internal class InternalPipelineController<T> : IPipelineController<T>
    {
        private readonly Dictionary<object, IPipeline<T>> __Pipelines = new();

        private PipelineDelegate<T>? __Next;
        private IPipeline<T>? __NowPipeline;
        private string? __Name;
        private List<string> InvokePathList = new();

        PipelineDelegate<T> IPipelineController<T>.NextPipeline
        {
            get
            {
                return __Next!;
            }
            set
            {
                __Next = value;
                __NowPipeline = null!;
            }
        }
        public string NextPipelineName
        {
            get
            {
                return __Name ?? "Unknow Pipeline";
            }
            set
            {
                __Name = value;
            }
        }

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
            StringBuilder stringBuilder = new();
            foreach (string pipeline in InvokePathList)
            {
                stringBuilder.AppendLine(pipeline);
            }
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
            InvokePathList.Add(NextPipelineName);
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
            if (__Pipelines.TryGetValue(pipelineName, out IPipeline<T>? val))
                __NowPipeline = val;
            return await Next(t);
        }
    }
}
