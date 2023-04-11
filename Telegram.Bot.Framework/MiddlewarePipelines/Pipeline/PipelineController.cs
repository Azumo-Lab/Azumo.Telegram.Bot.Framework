﻿//  <Telegram.Bot.Framework>
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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Pipeline
{
    /// <summary>
    /// 流水线控制器
    /// </summary>
    public class PipelineController : IPipelineController
    {
        private readonly Dictionary<string, IPipelineBuilder> __Pipelines = new();
        private MiddlewareDelegate __NextHandle = Session => Task.CompletedTask;

        /// <summary>
        /// 主分支的名称
        /// </summary>
        private string MainPipelineName;

        /// <summary>
        /// 切换流水线
        /// </summary>
        /// <param name="pipelineName">流水线名称，不输入即默认流水线</param>
        public void ChangePipeline(string pipelineName)
        {
            if (pipelineName.IsTrimEmpty())
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
            if (pipelineName.IsTrimEmpty())
                return;

            if (!__Pipelines.Any())
                MainPipelineName = pipelineName;

            if (!__Pipelines.ContainsKey(pipelineName))
                __Pipelines.Add(pipelineName, piplineBuilder);
        }

        /// <summary>
        /// 下一个工序
        /// </summary>
        /// <param name="Session">请求对话</param>
        /// <returns></returns>
        public async Task Next(ITelegramSession Session)
        {
            await __NextHandle(Session);
        }

        /// <summary>
        /// 设置下一个工序
        /// </summary>
        /// <param name="handle">中间件委托</param>
        void IPipelineController.SetNextHandle(MiddlewareDelegate handle)
        {
            __NextHandle = handle;
        }
    }
}
