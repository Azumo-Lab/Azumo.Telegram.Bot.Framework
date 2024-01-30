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
public abstract class TimerTaskExec : ITaskExec
{
    /// <summary>
    /// 定时任务的计时器
    /// </summary>
    private static readonly Timer __Timer = new(TimeSpan.FromSeconds(60));

    /// <summary>
    /// 定时任务执行时间
    /// </summary>
    protected virtual TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// 上次执行时间
    /// </summary>
    private DateTime __ExecTime = DateTime.MinValue;

    /// <summary>
    /// 下次执行时间
    /// </summary>
    private DateTime __NextTime = DateTime.MinValue;

    /// <summary>
    /// 静态初始化
    /// </summary>
    static TimerTaskExec() => __Timer.Start();

    /// <summary>
    /// 初始化
    /// </summary>
    public TimerTaskExec()
    {
        __ExecTime = DateTime.Now;
        __NextTime = DateTime.Now + TimeSpan;
    }

    /// <summary>
    /// 执行定时任务
    /// </summary>
    /// <returns></returns>
    public Task Execute()
    {
        __Timer.Elapsed += ExecuteElapsed!;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 定期执行的定时任务
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExecuteElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        var now = DateTime.Now;
        if (now > __ExecTime && now > __NextTime)
        {
            __ExecTime = now;
            _ = Exec().ConfigureAwait(false);
            __NextTime = DateTime.Now + TimeSpan;
        }
    }

    /// <summary>
    /// 实现的抽象定时任务
    /// </summary>
    /// <returns></returns>
    protected abstract Task Exec();
}
