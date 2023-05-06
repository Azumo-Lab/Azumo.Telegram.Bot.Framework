using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzumoLab.Downloader.Abstracts
{
    public interface IDownload
    {
        /// <summary>
        /// 添加一个下载任务
        /// </summary>
        /// <param name="url"></param>
        public void AddTask(string url);

        public Task DownloadWait();

        public Task DownloadWait(TimeSpan timeSpan);
    }
}
