//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Execs
{
    /// <summary>
    /// 后台任务
    /// </summary>
    public abstract class BackGroundTask : ITask
    {
        /// <summary>
        /// 发生错误的重试次数
        /// </summary>
        protected int ErrorCount { get; set; } = 3;

        /// <summary>
        /// 发生错误后的延迟时间
        /// </summary>
        protected TimeSpan ErrorDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 执行接口的任务
        /// </summary>
        /// <remarks>
        /// 向线程池中添加一个异步任务
        /// </remarks>
        /// <param name="input">传入参数</param>
        /// <param name="token">token</param>
        /// <returns>异步执行</returns>
        public virtual Task ExecuteAsync(object? input, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
                _ = ThreadPool.QueueUserWorkItem(async (inputObj) =>
                {
                    var errorCount = 0;
                ReExecute:
                    try
                    {
                        // 开始执行后台任务
                        if (!token.IsCancellationRequested)
                            await BackGroundExecuteAsync(inputObj, token);
                    }
                    catch (Exception) when (token.IsCancellationRequested)
                    {
                        return; // 立刻结束
                    }
                    catch (Exception)
                    {
                        errorCount++;
                        if (errorCount < ErrorCount)
                            try
                            {
                                await Task.Delay(ErrorDelay, token); // 重试
                            }
                            catch (TaskCanceledException)
                            {
                                return; // 立刻结束
                            }
                        else
                            return; // 重试后放弃
                        goto ReExecute;
                    }
                }, input);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 开始执行后台任务
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <param name="token">Token <see cref="CancellationToken"/></param>
        /// <returns>异步任务</returns>
        protected abstract Task BackGroundExecuteAsync(object? input, CancellationToken token);
    }
}
