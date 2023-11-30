using Timer = System.Timers.Timer;

namespace Telegram.Bot.Framework.Abstracts.Exec
{
    /// <summary>
    /// 定时任务
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public abstract class AbsTimerExec : IExec
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Timer __Timer = new(TimeSpan.FromSeconds(60));

        /// <summary>
        /// 
        /// </summary>
        protected virtual TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// 
        /// </summary>
        private DateTime __ExecTime = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        private DateTime __NextTime = DateTime.MinValue;

        /// <summary>
        /// 
        /// </summary>
        static AbsTimerExec()
        {
            __Timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public AbsTimerExec()
        {
            __ExecTime = DateTime.Now;
            __NextTime = DateTime.Now + TimeSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Execute()
        {
            __Timer.Elapsed += ExecuteElapsed!;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            if (now > __ExecTime && now > __NextTime)
            {
                __ExecTime = now;
                _ = Exec().ConfigureAwait(false);
                __NextTime = DateTime.Now + TimeSpan;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract Task Exec();
    }
}
