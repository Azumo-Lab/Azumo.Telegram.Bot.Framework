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
/// 按定时间隔执行的任务
/// </summary>
public abstract class TimerTask : BackGroundTask
{
    /// <summary>
    /// 任务执行的时间
    /// </summary>
    private DateTime ExecuteDatetime { get; set; }

    /// <summary>
    /// 任务执行的间隔
    /// </summary>
    protected TimeSpan Interval { get; set; }

    /// <summary>
    /// 开始执行后台任务
    /// </summary>
    /// <param name="input">传入参数</param>
    /// <param name="token">Token</param>
    /// <returns>异步任务</returns>
    protected override async Task BackGroundExecuteAsync(object? input, CancellationToken token)
    {
        // 如果任务没有取消，则始终执行
        while (!token.IsCancellationRequested)
        {
            // 如果当前时间大于等于执行时间，则执行任务
            if (DateTime.Now >= ExecuteDatetime)
            {
                // 计算下一次执行的时间
                ExecuteDatetime = DateTime.Now.Add(Interval);
                await IntervalExecuteAsync(input, token);
            }
            else
            {
                await Task.Delay(Interval, token);
            }
        }
    }

    /// <summary>
    /// 执行定期间隔的任务
    /// </summary>
    /// <param name="input">传入参数</param>
    /// <param name="token">Token</param>
    /// <returns>异步任务</returns>
    protected abstract Task IntervalExecuteAsync(object? input, CancellationToken token);
}
