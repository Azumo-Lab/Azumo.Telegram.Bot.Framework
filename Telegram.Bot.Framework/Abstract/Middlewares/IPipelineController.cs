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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Sessions;

namespace Telegram.Bot.Framework.Abstract.Middlewares
{
    /// <summary>
    /// 流水线控制器
    /// </summary>
    public interface IPipelineController
    {
        #region 框架内部方法
        internal void SetNextHandle(MiddlewareDelegate handle);
        #endregion

        /// <summary>
        /// 判断是否已经有了内容
        /// </summary>
        public bool HasAnyPipeline { get; }

        /// <summary>
        /// 执行下一条作业
        /// </summary>
        /// <returns></returns>
        public Task Next(ITelegramChat Chat);

        /// <summary>
        /// 切换一条流水线
        /// </summary>
        /// <param name="pipelineName">名称</param>
        public void ChangePipeline(string pipelineName = default);

        /// <summary>
        /// 创建一条新的流水线
        /// </summary>
        /// <param name="pipelineName">名称</param>
        /// <param name="piplineBuilder"></param>
        public void AddPipeline(string pipelineName, IPipelineBuilder piplineBuilder);

        /// <summary>
        /// 创建一条新的流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="piplineBuilder"></param>
        /// <returns></returns>
        public bool TryAddPipeline(string pipelineName, IPipelineBuilder piplineBuilder);
    }
}
