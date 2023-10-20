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
        /// <summary>
        /// 流水线字典
        /// </summary>
        private readonly Dictionary<object, IPipeline<T>> __Pipelines = new();

        /// <summary>
        /// 下一道工序
        /// </summary>
        private PipelineDelegate<T>? __Next;

        /// <summary>
        /// 当前使用的流水线
        /// </summary>
        private IPipeline<T>? __NowPipeline;

        /// <summary>
        /// 执行工序名称
        /// </summary>
        private string? __Name;

        /// <summary>
        /// 流水线执行链
        /// </summary>
        private List<string> InvokePathList = new();

        /// <summary>
        /// 下一个工序
        /// </summary>
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

        /// <summary>
        /// 下一道工序的名称
        /// </summary>
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
        /// 流水线执行结果
        /// </summary>
        public PipelineResultEnum PipelineResultEnum { get; set; } = PipelineResultEnum.Success;

        /// <summary>
        /// 添加一条流水线
        /// </summary>
        /// <typeparam name="PipelineNameType">流水线名称类型，可以是文本，数字，枚举</typeparam>
        /// <param name="pipelineName">流水线名称</param>
        /// <param name="pipeline">流水线类</param>
        public void AddPipeline<PipelineNameType>(PipelineNameType pipelineName, IPipeline<T> pipeline) where PipelineNameType : notnull
        {
            __Pipelines.Add(pipelineName, pipeline);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 进行下一步的流水线处理操作
        /// </summary>
        /// <param name="t">处理数据</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<T> NextAsync(T t)
        {
            InvokePathList.Add(NextPipelineName);
            if (__NowPipeline != null)
                return await __NowPipeline.Invoke(t);
            else if (__Next != null)
                return await __Next(t, this);

            throw new NullReferenceException();
        }

        /// <summary>
        /// 停止执行流水线
        /// </summary>
        /// <param name="t">处理数据</param>
        /// <returns>直接返回处理数据</returns>
        public Task<T> StopAsync(T t)
        {
            return Task.FromResult(t);
        }

        /// <summary>
        /// 切换执行的流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<T> SwitchTo<PipelineNameType>(PipelineNameType pipelineName, T t) where PipelineNameType : notnull
        {
            if (__Pipelines.TryGetValue(pipelineName, out IPipeline<T>? val))
                __NowPipeline = val;
            return await NextAsync(t);
        }
    }
}
