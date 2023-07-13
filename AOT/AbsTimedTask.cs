using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AOT
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbsTimedTask : ITimedTask, IDisposable
    {
        #region 私有变量
        private readonly ReaderWriterLockSlim __Lock = new();
        private bool __Running;
        private DateTime __PrevInvokeTime;
        private DateTime __NextInvokeTime;
        private static readonly Timer __InvokeTImer = new(TimeSpan.FromSeconds(1));
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

        public async Task Exec()
        {
            UpdateInvokeTime();

            __InvokeTImer.Elapsed += InvokeTImer_Elapsed;
            __InvokeTImer.Start();

            await Task.CompletedTask;
        }

        private void InvokeTImer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            __Lock.EnterReadLock();
            bool runningFlag = __Running;
            __Lock.ExitReadLock();

            if (__NextInvokeTime < DateTime.Now && !runningFlag)
                InvokeExec().ConfigureAwait(true).GetAwaiter().GetResult();
        }

        private async Task InvokeExec()
        {
            RunFlag(true);
            await Invoke();

            UpdateInvokeTime();
            RunFlag(false);
        }

        private void UpdateInvokeTime()
        {
            __Lock.EnterWriteLock();
            __PrevInvokeTime = DateTime.Now;
            __NextInvokeTime = __PrevInvokeTime + GetTimeSpan();
            __Lock.ExitWriteLock();
        }

        private void RunFlag(bool flag)
        {
            __Lock.EnterWriteLock();
            __Running = flag;
            __Lock.ExitWriteLock();
        }

        void IDisposable.Dispose()
        {
            __InvokeTImer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
