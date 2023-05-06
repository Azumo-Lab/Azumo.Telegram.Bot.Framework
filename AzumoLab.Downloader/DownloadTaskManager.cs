using AzumoLab.Downloader.Abstracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AzumoLab.Downloader
{
    internal class DownloadTaskManager : ITaskManager
    {
        private int __Count = 0;
        private readonly List<IDownloadTask> __DownloadTasks = new List<IDownloadTask>();
        private readonly ReaderWriterLockSlim __Lock = new ReaderWriterLockSlim();

        private readonly ThreadLocal<int> __ThreadIndex = new ThreadLocal<int>();

        public void AddTask(IDownloadTask downloadTask)
        {
            __Lock.EnterWriteLock();
            __DownloadTasks.Add(downloadTask);
            __Count++;
            __Lock.ExitWriteLock();
        }

        public IDownloadTask GetTask()
        {
            try
            {
                if (__Count <= __ThreadIndex.Value)
                    return null!;
                return __DownloadTasks[__ThreadIndex.Value];
            }
            finally
            {
                __ThreadIndex.Value++;
            }
            
        }
    }
}
