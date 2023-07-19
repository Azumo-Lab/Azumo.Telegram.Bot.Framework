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

namespace Telegram.Bot.Framework.Pipeline.Abstracts
{
    /// <summary>
    /// 流水线管理控制器
    /// </summary>
    public interface IPipelineController<T>
    {
        /// <summary>
        /// 添加一条流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <param name="pipeline"></param>
        public void AddPipeline<PipelineNameType>(PipelineNameType pipelineName, IPipeline<T> pipeline) where PipelineNameType : notnull;

        /// <summary>
        /// 设置下一道工序
        /// </summary>
        /// <param name="pipelineDelegate"></param>
        internal void SetNext(PipelineDelegate<T> pipelineDelegate);

        /// <summary>
        /// 获取执行路径
        /// </summary>
        /// <returns></returns>
        public string GetInvokePath();

        #region 控制器控制

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

        /// <summary>
        /// 切换到指定流水线
        /// </summary>
        /// <param name="pipelineName"></param>
        /// <returns></returns>
        public Task<T> SwitchTo<PipelineNameType>(PipelineNameType pipelineName, T t) where PipelineNameType : notnull;

        #endregion
    }
}
