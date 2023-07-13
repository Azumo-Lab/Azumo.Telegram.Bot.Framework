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

namespace Pipeline.Framework.Abstracts
{
    /// <summary>
    /// 流水线管理控制器
    /// </summary>
    public interface IPipelineController<T>
    {
        /// <summary>
        /// 切换一条流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        public void ChangePipeline(string pipelineName);

        /// <summary>
        /// 添加一条流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="pipeline"></param>
        public void AddPipeline(string pipelineName, IPipeline<T> pipeline);

        /// <summary>
        /// 设置下一道工序
        /// </summary>
        /// <param name="pipelineDelegate"></param>
        internal void SetNext(PipelineDelegate<T> pipelineDelegate);

        /// <summary>
        /// 执行下一道工序
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Task<T> Next(T t);

        /// <summary>
        /// 停止当前流水线并立刻返回值
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Task<T> Stop(T t);
    }
}
