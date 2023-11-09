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

namespace Azumo.Pipeline.Abstracts
{
    /// <summary>
    /// 处理任务
    /// </summary>
    /// <remarks>
    /// 流水线的一个处理任务
    /// </remarks>
    /// <typeparam name="T">要进行处理的类型</typeparam>
    public interface IProcessAsync<T>
    {
        /// <summary>
        /// 执行处理任务
        /// </summary>
        /// <param name="t">任务处理的数据类型</param>
        /// <param name="pipelineController">流水线控制器</param>
        /// <returns>异步的处理后的数据</returns>
        public Task<T> ExecuteAsync(T t, IPipelineController<T> pipelineController);
    }
}
