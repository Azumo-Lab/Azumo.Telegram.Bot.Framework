using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Telegram.Bot.Framework.Interfaces
{
    public abstract class AbsTimerExec : IExec
    {
        private static readonly Timer __Timer = new(TimeSpan.FromSeconds(60));

        protected virtual TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(60);

        private DateTime __ExecTime = DateTime.MinValue;
        private DateTime __NextTime = DateTime.MinValue;

        static AbsTimerExec()
        {
            __Timer.Start();
        }

        public AbsTimerExec()
        {
            __ExecTime = DateTime.Now;
            __NextTime = DateTime.Now + TimeSpan;
        }

        public Task Execute()
        {
            __Timer.Elapsed += ExecuteElapsed;
            return Task.CompletedTask;
        }

        private void ExecuteElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            if (now > __ExecTime && now > __NextTime)
            {
                __ExecTime = now;
                Exec().ConfigureAwait(false);
                __NextTime = DateTime.Now + TimeSpan;
            }
        }

        protected abstract Task Exec();
    }
}
