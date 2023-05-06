using System;
using System.Collections.Generic;
using System.Text;

namespace AzumoLab.Downloader.Abstracts
{
    public interface IDownloadTask
    {
        public string Url { get; }

        public bool IsCompleted { get; set; }

        public long FileSize { get; set; }

        public string FileName { get; set; }

        public bool CanRange { get; set; }

        public IFileSlice FileSlice { get; set; }
    }
}
