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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Telegram.Bot.Framework.Abstract.BackgroundProcess
{
    /// <summary>
    /// 用于实现定时执行任务的类
    /// </summary>
    /// <remarks>
    /// 没有什么太深奥的东西，只需要继承之后即可使用
    /// </remarks>
    public abstract class AbsTimedTask : ITimedTask
    {
        #region 私有变量
        private readonly ReaderWriterLockSlim __Lock = new();
        private bool __Running;
        private DateTime __PrevInvokeTime;
        private DateTime __NextInvokeTime;
        private static readonly Timer __InvokeTImer = new(TimeSpan.FromSeconds(1));
        #endregion

        #region 公用变量
        /// <summary>
        /// 指示程序的执行间隔
        /// </summary>
        /// <remarks>
        /// 执行间隔，多长时间检测一次程序是否执行，默认为1秒钟时间<br></br>
        /// 这是一个全局变量，会影响所有的计时任务的执行
        /// </remarks>
        public static TimeSpan Interval
        {
            get
            {
                return TimeSpan.FromMilliseconds(__InvokeTImer.Interval);
            }
            set
            {
                __InvokeTImer.Interval = value.Milliseconds;
            }
        }
        #endregion
        /// <summary>
        /// 获取执行间隔
        /// </summary>
        /// <remarks>
        /// 通过这个方法获得执行间隔，例如，每10秒执行一次
        /// </remarks>
        /// <returns>执行间隔</returns>
        public abstract TimeSpan GetTimeSpan();

        /// <summary>
        /// 执行的内容
        /// </summary>
        /// <remarks>
        /// 这里写执行的主体内容
        /// </remarks>
        /// <returns>异步</returns>
        protected abstract Task Invoke();

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns>异步任务</returns>
        public async Task Exec()
        {
            UpdateInvokeTime();

            __InvokeTImer.Elapsed += InvokeTImer_Elapsed;

            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvokeTImer_Elapsed(object sender, ElapsedEventArgs e)
        {
            __Lock.EnterReadLock();
            bool runningFlag = __Running;
            __Lock.ExitReadLock();

            if (__NextInvokeTime < DateTime.Now && !runningFlag)
                InvokeExec().ConfigureAwait(true).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task InvokeExec()
        {
            RunFlag(true);
            await Invoke();

            UpdateInvokeTime();
            RunFlag(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateInvokeTime()
        {
            __Lock.EnterWriteLock();
            __PrevInvokeTime = DateTime.Now;
            __NextInvokeTime = __PrevInvokeTime + GetTimeSpan();
            __Lock.ExitWriteLock();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void RunFlag(bool flag)
        {
            __Lock.EnterWriteLock();
            __Running = flag;
            __Lock.ExitWriteLock();
        }
    }
}
