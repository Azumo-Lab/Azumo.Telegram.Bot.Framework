using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AzumoLab.Downloader
{
    public class DownloadProgress : IDisposable
    {
        private readonly long __FileSize;
        private readonly string __FileName;

        private readonly Timer __Timer;
        private long __Progress;

        static DownloadProgress()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        
        public DownloadProgress(long fileSize, string fileName)
        {
            __FileSize = fileSize;
            __FileName = fileName;  
            __Timer = new Timer(500);
            __Timer.Elapsed += Timer_Elapsed;
            __Timer.Start();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"下载中：{__FileName}");
            long now = Interlocked.Read(ref __Progress);
            double progress = ((double)now / __FileSize) * 100;
            for (int i = 0; i < 10; i++)
            {
                int c = (int)(progress / 10);
                if (i < c)
                    Console.Write(">");
                else
                    Console.Write("-");
            }
            Console.Write($"{progress.ToString("00.00")} %");
            Console.WriteLine();
        }

        public void SetProgress(long progress)
        {
            Interlocked.Add(ref __Progress, progress);
        }

        public void Dispose()
        {
            Timer_Elapsed(null, null!);
            GC.SuppressFinalize(this);
            ((IDisposable)__Timer).Dispose();
        }
    }
}
