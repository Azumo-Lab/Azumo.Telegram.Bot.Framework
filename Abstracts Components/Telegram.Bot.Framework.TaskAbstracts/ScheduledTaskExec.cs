//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Timer = System.Timers.Timer;

namespace Telegram.Bot.Framework.TaskAbstracts;
public abstract class ScheduledTaskExec : ITaskExec
{
    /// <summary>
    /// 定时任务的计时器
    /// </summary>
    private static readonly Timer __Timer = new(TimeSpan.FromSeconds(30));

    /// <summary>
    /// 设定执行的时间
    /// </summary>
    protected List<DateTime> Scheduled { get; private set; } = [];

    /// <summary>
    /// 按年来循环
    /// </summary>
    protected bool YearLoop { get; set; }

    /// <summary>
    /// 按月来循环
    /// </summary>
    protected bool MonthLoop { get; set; }

    /// <summary>
    /// 按天来循环
    /// </summary>
    protected bool DayLoop { get; set; } = true;

    /// <summary>
    /// 按星期来循环
    /// </summary>
    protected bool WeekLoop { get; set; }

    /// <summary>
    /// 下次执行时间
    /// </summary>
    private DateTime __NextTime = DateTime.MinValue;

    /// <summary>
    /// 静态初始化，全局计时器启动
    /// </summary>
    static ScheduledTaskExec() => __Timer.Start();

    /// <summary>
    /// 开始执行
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task Execute()
    {
        if (Scheduled == null || Scheduled.Count == 0)
            throw new InvalidOperationException("请添加定时时间");

        Scheduled = [.. Scheduled.OrderBy(x => x)];
        __NextTime = NextInvokeTime();
        __Timer.Elapsed += new System.Timers.ElapsedEventHandler((obj, e) =>
        {
            if (DateTime.Now >= __NextTime)
                _ = Exec();
            else
                return;
            __NextTime = NextInvokeTime();
        });
        return Task.CompletedTask;
    }

    /// <summary>
    /// 计算下一次执行的时间
    /// </summary>
    /// <returns></returns>
    protected virtual DateTime NextInvokeTime()
    {
        var next = Scheduled.First();
        Scheduled.Add(AddDateTime(next));
        Scheduled.RemoveAt(0);
        return next;
    }

    private DateTime AddDateTime(DateTime next)
    {
        var result = DayLoop ? next.AddDays(1) : WeekLoop ? next.AddDays(7) : MonthLoop ? next.AddMonths(1) : YearLoop ? next.AddYears(1) : next;
        return result;
    }

    protected void SetSchedule(DateTime dateTime)
    {
        if (DateTime.Now >= dateTime)
            dateTime = AddDateTime(dateTime);
        Scheduled.Add(dateTime);
    }

    /// <summary>
    /// 实现的抽象定时任务
    /// </summary>
    /// <returns></returns>
    protected abstract Task Exec();
}
