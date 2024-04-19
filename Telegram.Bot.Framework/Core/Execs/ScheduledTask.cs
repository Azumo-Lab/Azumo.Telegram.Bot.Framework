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
/// 
/// </summary>
public abstract class ScheduledTask : BackgroundTask
{
    /// <summary>
    /// 
    /// </summary>
    private readonly List<DateTime?> InvokeTimes = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="token"></param>
    /// <returns></returns>
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
                await ScheduledExecuteAsync(input, token);
                InvokeTimes.Remove(time);
                var newTime = time.Value.AddDays(1);
                InvokeTimes.Add(newTime);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    protected abstract Task ScheduledExecuteAsync(object? input, CancellationToken token);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    protected void AddScheduled(DateTime dateTime) => 
        InvokeTimes.Add(dateTime);
}
