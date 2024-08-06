//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
//
//  Author: 牛奶

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Execs;
using Telegram.Bot.Framework.InternalCore.Attritubes;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.TelegramBotProc
{
    /// <summary>
    /// 
    /// </summary>
    [TelegramBotStartProc]
    internal class InvokeStartTask : IMiddleware<IServiceProvider, Task>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        public async Task Execute(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
        {
            var logger = input.GetService<ILogger<InvokeStartTask>>();
            var tasks = input.GetServices<IStartTask>().ToList();
            var token = input.GetRequiredService<CancellationTokenSource>().Token;
            var tasksCount = tasks.Count;
            if (tasksCount != 0)
            {
                logger?.LogInformation("总共找到 {A0} 个任务", tasksCount);
                foreach (var task in tasks)
                {
                    var name = (task as IName)?.Name;
                    if (!string.IsNullOrEmpty(name))
                        logger?.LogInformation("开始执行任务：{A0}", name);

                    try
                    {
                        await task.ExecuteAsync(null, token);
                    }
                    catch (Exception)
                    {
                        if (!string.IsNullOrEmpty(name))
                            logger?.LogError("任务执行失败：{A0}", name);
                    }
                }
            }

            await Next(input);
        }
    }
}
