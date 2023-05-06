using AzumoLab.Downloader.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzumoLab.Downloader
{
    internal class DownloadTask : IDownloadTask
    {
        public string Url { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DownloadTask(string url, bool isCompleted = false)
        {
            Url = url;
            IsCompleted = isCompleted;
        }
    }
}
