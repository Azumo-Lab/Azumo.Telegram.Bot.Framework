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

namespace Telegram.Bot.Framework.Core.Execs;

/// <summary>
/// 后台任务
/// </summary>
public abstract class BackGroundTask : ITask
{
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
                    await BackGroundExecuteAsync(inputObj, token);
                }
                catch (TaskCanceledException)
                {
                    // 立刻结束
                    return;
                }
                catch (Exception) when (token.IsCancellationRequested)
                {
                    // 立刻结束
                    return;
                }
                catch (Exception)
                {
                    errorCount++;
                    if (errorCount < 3)
                    {
                        // 重试3次
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        goto ReExecute;
                    }
                    else
                    {
                        // 重试3次后放弃
                        return;
                    }
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
