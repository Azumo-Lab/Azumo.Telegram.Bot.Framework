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

using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Process;
using Telegram.Bot.Framework.Abstracts.Services;

namespace Telegram.Bot.Framework.InternalProc.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class TaskService : ITaskService
    {
        private readonly Dictionary<string, ITimedTask> __TimedTask = new();

        public void AddTask(string taskName, ITimedTask timedTask)
        {
            __TimedTask.Add(taskName, timedTask);
        }

        public void Remove(string taskName)
        {
            _ = __TimedTask.Remove(taskName);
        }

        public async Task StartAsync(string taskName)
        {
            if (__TimedTask.TryGetValue(taskName, out ITimedTask task))
                await task.StartAsync();
        }

        public async Task StartAllAsync()
        {
            foreach (ITimedTask task in __TimedTask.Values)
                await task.StartAsync();
        }

        public async Task StopAsync(string taskName)
        {
            if (__TimedTask.TryGetValue(taskName, out ITimedTask task))
                await task.StopAsync();
        }

        public async Task StopAllAsync()
        {
            foreach (ITimedTask task in __TimedTask.Values)
                await task.StopAsync();
        }
    }
}
