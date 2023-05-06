using System;
using System.Collections.Generic;
using System.Text;

namespace AzumoLab.Downloader.Abstracts
{
    public interface ITaskManager
    {
        public void AddTask(IDownloadTask downloadTask);

        public IDownloadTask GetTask();
    }
}
