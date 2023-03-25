using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace Telegram.Bot.Framework.Channel.Abstract
{
    internal abstract class AbsBackgroundWorker : IBackgroundWorker, IDisposable
    {
        private static BackgroundWorker BackgroundWorker { get; } = new BackgroundWorker();

        public virtual void Invoke(params object[] objects)
        {
            BackgroundWorker.RunWorkerAsync(objects);
        }

        public AbsBackgroundWorker()
        {
            AddTask();
        }

        private void AddTask()
        {
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
        }

        private void RemoveTask()
        {
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
        }

        protected abstract void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e);

        public void Dispose()
        {
            RemoveTask();
        }
    }
}
