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
/// 按计划执行的任务
/// </summary>
public abstract class ScheduledTask : BackGroundTask
{
    /// <summary>
    /// 执行任务时间表
    /// </summary>
    private readonly List<DateTime?> InvokeTimes = [];

    /// <summary>
    /// 一组计划之间的间隔
    /// </summary>
    /// <remarks>
    /// 例如，这个值是一天的情况下，任务将会每天执行一次。
    /// </remarks>
    protected TimeSpan IntervalTimeSpan { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// 开始执行异步任务
    /// </summary>
    /// <param name="input">传入参数</param>
    /// <param name="token">Token</param>
    /// <returns>异步任务</returns>
    protected override async Task BackGroundExecuteAsync(object? input, CancellationToken token)
    {
        var orderTimes = InvokeTimes.OrderBy(x => x).ToList();
        InvokeTimes.Clear();
        InvokeTimes.AddRange(orderTimes);

        while (!token.IsCancellationRequested)
        {
            var time = InvokeTimes.FirstOrDefault();
            if (time == null)
                return;

            if (DateTime.Now >= time)
            {
                // 执行任务
                await ScheduledExecuteAsync(input, token);
                InvokeTimes.Remove(time);
                var newTime = time.Value.Add(IntervalTimeSpan);
                
                // 添加新的执行时间
                if (newTime > DateTime.Now)
                    InvokeTimes.Add(newTime);
            }

            await Task.Delay(TimeSpan.FromSeconds(60), token);
        }
    }

    /// <summary>
    /// 任务计划表执行的任务
    /// </summary>
    /// <param name="input">参数</param>
    /// <param name="token">Token</param>
    /// <returns></returns>
    protected abstract Task ScheduledExecuteAsync(object? input, CancellationToken token);

    /// <summary>
    /// 添加一个计划时间
    /// </summary>
    /// <param name="hour">小时</param>
    /// <param name="minute">分钟</param>
    /// <param name="second">秒</param>
    protected void AddScheduled(int hour, int minute = 0, int second = 0) => 
        InvokeTimes.Add(DateTime.Now.Date.AddHours(hour).AddMinutes(minute).AddSeconds(second));
}
