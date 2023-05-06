using System;
using System.Collections.Generic;
using System.Text;

namespace AzumoLab.Downloader.Abstracts
{
    public interface IFileSlice
    {
        public (long StartIndex, long EndIndex, int FileIndex) GetSlice();
    }
}
