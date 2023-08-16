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

using Telegram.Bot.Framework.Abstracts.Process;

namespace Telegram.Bot.Framework.Abstracts.Services
{
    /// <summary>
    /// 任务管理服务
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="timedTask"></param>
        public void AddTask(string taskName, ITimedTask timedTask);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        public Task StopAsync(string taskName);

        /// <summary>
        /// 
        /// </summary>
        public Task StopAllAsync();

        /// <summary>
        /// 
        /// </summary>
        public Task StartAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        public Task StartAsync(string taskName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        public void Remove(string taskName);
    }
}
