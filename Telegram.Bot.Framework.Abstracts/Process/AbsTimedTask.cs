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

namespace Telegram.Bot.Framework.Abstracts.Process
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

        /// <summary>
        /// 用于加锁
        /// </summary>
        private readonly ReaderWriterLockSlim __Lock = new();

        /// <summary>
        /// 用于指示该任务是否正在执行
        /// </summary>
        private bool __Running;

        /// <summary>
        /// 上一次的执行时间
        /// </summary>
        private DateTime __PrevInvokeTime;

        /// <summary>
        /// 下一次的执行时间
        /// </summary>
        private DateTime __NextInvokeTime;

        /// <summary>
        /// 一个计时器
        /// </summary>
        /// <remarks>
        /// 这个计时器是用于计时任务的执行
        /// </remarks>
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
            get => TimeSpan.FromMilliseconds(__InvokeTImer.Interval);
            set => __InvokeTImer.Interval = value.Milliseconds;
        }
        #endregion

        /// <summary>
        /// 静态初始化
        /// </summary>
        static AbsTimedTask()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler((obj, e) =>
            {
                __InvokeTImer.Stop();
                __InvokeTImer.Dispose();
            });
            // 一个默认实现的方法
            __InvokeTImer.Elapsed += new ElapsedEventHandler((obj, e) => { });
            // 启动
            __InvokeTImer.Start();
        }

        /// <summary>
        /// 获取执行间隔
        /// </summary>
        /// <remarks>
        /// 通过这个方法获得执行间隔，例如，每10秒执行一次
        /// </remarks>
        /// <returns>执行间隔</returns>
        protected abstract TimeSpan GetTimeSpan();

        /// <summary>
        /// 执行的内容
        /// </summary>
        /// <remarks>
        /// 这里写执行的主体内容
        /// </remarks>
        /// <returns>异步</returns>
        protected abstract Task Invoke();

        /// <summary>
        /// 计时循环的主体部分
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
        /// 开始执行计时任务的部分
        /// </summary>
        /// <returns></returns>
        private async Task InvokeExec()
        {
            UpdateRunFlag(true);
            await Invoke();

            UpdateInvokeTime();
            UpdateRunFlag(false);
        }

        /// <summary>
        /// 更新执行的时间
        /// </summary>
        private void UpdateInvokeTime()
        {
            __Lock.EnterWriteLock();
            __PrevInvokeTime = DateTime.Now;
            __NextInvokeTime = __PrevInvokeTime + GetTimeSpan();
            __Lock.ExitWriteLock();
        }

        /// <summary>
        /// 更新运行Flag
        /// </summary>
        /// <param name="flag"></param>
        private void UpdateRunFlag(bool flag)
        {
            __Lock.EnterWriteLock();
            __Running = flag;
            __Lock.ExitWriteLock();
        }

        /// <summary>
        /// 停止执行当前任务
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            __InvokeTImer.Elapsed -= InvokeTImer_Elapsed!;
            await Task.CompletedTask;
        }

        /// <summary>
        /// 开始执行当前任务
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            UpdateInvokeTime();
            __InvokeTImer.Elapsed += InvokeTImer_Elapsed!;
            await Task.CompletedTask;
        }
    }
}
