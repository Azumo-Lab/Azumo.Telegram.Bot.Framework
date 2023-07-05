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

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.ExtensionMethods;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Pipeline
{
    /// <summary>
    /// 流水线控制器的实现类
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped)]
    public class PipelineController : IPipelineController
    {
        private readonly Dictionary<string, IPipelineBuilder> __Pipelines = new();
        private MiddlewareDelegate __NextHandle = Session => Task.CompletedTask;

        /// <summary>
        /// 主分支的名称
        /// </summary>
        private string MainPipelineName;

        /// <summary>
        /// 判断当前的流水线是否具有流水线步骤
        /// </summary>
        public bool HasAnyPipeline => __Pipelines.Count != 0;

        /// <summary>
        /// 切换流水线
        /// </summary>
        /// <param name="pipelineName">流水线名称，不输入即默认流水线</param>
        public void ChangePipeline(string pipelineName)
        {
            if (pipelineName.IsNullOrTrimEmpty())
                pipelineName = MainPipelineName;

            if (__Pipelines.TryGetValue(pipelineName, out IPipelineBuilder builder))
                __NextHandle = builder.Builder(this);
        }

        /// <summary>
        /// 添加一条新的流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="piplineBuilder"></param>
        public void AddPipeline(string pipelineName, IPipelineBuilder piplineBuilder)
        {
            if (pipelineName.IsNullOrTrimEmpty())
                return;

            if (!__Pipelines.Any())
                MainPipelineName = pipelineName;

            if (!__Pipelines.ContainsKey(pipelineName))
                __Pipelines.Add(pipelineName, piplineBuilder);
        }

        /// <summary>
        /// 下一个工序
        /// </summary>
        /// <param name="Chat">请求对话</param>
        /// <returns></returns>
        public async Task Next(ITelegramChat Chat)
        {
            await __NextHandle(Chat);
        }

        /// <summary>
        /// 设置下一个工序
        /// </summary>
        /// <param name="handle">中间件委托</param>
        void IPipelineController.SetNextHandle(MiddlewareDelegate handle)
        {
            __NextHandle = handle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="piplineBuilder"></param>
        /// <returns></returns>
        public bool TryAddPipeline(string pipelineName, IPipelineBuilder piplineBuilder)
        {
            bool result = false;
            if (pipelineName.IsNullOrEmpty())
                return result;

            if (__Pipelines.ContainsKey(pipelineName))
                return result;

            if (!__Pipelines.Any())
                MainPipelineName = pipelineName;

            if (result = !__Pipelines.ContainsKey(pipelineName))
                __Pipelines.Add(pipelineName, piplineBuilder);

            return result;
        }
    }
}
