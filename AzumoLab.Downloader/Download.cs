using AzumoLab.Downloader.Abstracts;
using AzumoLab.Downloader.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AzumoLab.Downloader
{
    public class Download : IDownload
    {
        private readonly ITaskManager __TaskManager = new DownloadTaskManager();
        private readonly HttpClient __HttpClient;
        private IDownloadTask Downloading;

        private const int CACHE_SIZE = 1024;

        #region 下载设置

        private int __Threads = int.MinValue;
        public int Threads
        {
            get
            {
                if (__Threads == int.MinValue)
                    throw new InvalidOperationException();
                return __Threads;
            }
            set
            {
                if (value <= 0 || value > 64)
                    throw new ArgumentException();
                __Threads = value;
            }
        }
        private string __SavePath = string.Empty;
        public string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(__SavePath))
                    throw new InvalidOperationException();
                return __SavePath;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException();
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                __SavePath = value;
            }
        }

        private int __Attempts = 1;
        public int Attempts
        {
            get
            {
                return __Attempts;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                __Attempts = value;
            }
        }

        #endregion

        public Download(HttpMessageHandler httpMessageHandler)
        {
            __HttpClient = new HttpClient(httpMessageHandler);
        }

        public Download()
        {
            __HttpClient = new HttpClient();
        }

        public void AddTask(string url)
        {
            IDownloadTask task = new DownloadTask(url);
            __TaskManager.AddTask(task);
        }

        public Task DownloadWait()
        {
            return DownloadWait(TimeSpan.FromDays(1));
        }

        public Task DownloadWait(TimeSpan timeSpan)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeSpan);
            return Task.Run(DownloadAction, cancellationTokenSource.Token);
        }

        private void DownloadAction()
        {
            Span<byte> byteCache = stackalloc byte[1024];
            while ((Downloading = __TaskManager.GetTask()) != null)
            {
                int attempts = 0;
                bool useThreads = Threads != 1;
            RESTART:
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Downloading.Url);
                bool isOK = false;
                if (useThreads)
                    isOK = DownloadWithOneThread(httpRequestMessage, ref byteCache);
                else
                    isOK = DownloadWithThreads(httpRequestMessage, ref byteCache, ref useThreads);
                
                if (isOK || attempts >= __Attempts)
                    continue;
                else
                {
                    attempts++;
                    goto RESTART;
                }
                    
            }
        }

        private bool DownloadWithOneThread(HttpRequestMessage httpRequestMessage, ref Span<byte> byteCache)
        {
            try
            {
                HttpResponseMessage responseMessage = __HttpClient.SendAsync(httpRequestMessage).Result;

                GetFileInfo(responseMessage);

                BufferedStream bufferedStream = new BufferedStream(responseMessage.Content.ReadAsStreamAsync().Result, byteCache.Length);
                BufferedStream fileStream = new BufferedStream(new FileStream(SavePath, FileMode.OpenOrCreate), byteCache.Length);

                int len;
                while ((len = bufferedStream.Read(byteCache)) > 0)
                {
                    Span<byte> cache = byteCache[..len];
                    fileStream.Write(cache);
                }

                bufferedStream.Close();
                bufferedStream.Dispose();

                fileStream.Close();
                fileStream.Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DownloadWithThreads(HttpRequestMessage httpRequestMessage, ref Span<byte> byteCache, ref bool usethreads)
        {
            try
            {
                IsSupportRange(httpRequestMessage);

                HttpResponseMessage responseMessage = __HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).Result;

                GetFileInfo(responseMessage);

                List<Action> actions = new List<Action>();
                for (int i = 0; i < Threads; i++)
                {
                    actions.Add(DownloadThreadAction);
                }

                Parallel.Invoke(actions.ToArray());

                Span<byte> cache = stackalloc byte[CACHE_SIZE];
                List<BufferedStream> files = Directory.GetFiles(SavePath, $"{Downloading.FileName}*.azumolabitem", SearchOption.TopDirectoryOnly)
                    .Select(x => new BufferedStream(new FileStream(x, FileMode.Open), CACHE_SIZE))
                    .ToList();
                BufferedStream writeFile = new BufferedStream(new FileStream(Path.Combine(SavePath, Downloading.FileName), FileMode.OpenOrCreate), CACHE_SIZE);
                foreach (BufferedStream file in files)
                {
                    int len;
                    while ((len = file.Read(cache)) > 0)
                    {
                        Span<byte> cacheLen = cache[..len];
                        writeFile.Write(cacheLen);
                    }
                    file.Dispose();
                }
                writeFile.Dispose();

                return true;
            }
            catch (NoSupportRange)
            {
                usethreads = false;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private void DownloadThreadAction()
        {
            (long Start, long End, int IndexStr) = Downloading.FileSlice.GetSlice();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, Downloading.Url);
            httpRequestMessage.Content = new StringContent(string.Empty);
            httpRequestMessage.Headers.Range = new RangeHeaderValue(Start, End);

            Span<byte> _DownloadCache = stackalloc byte[1024];
            string savePath = Path.Combine(SavePath, $"{Downloading.FileName}_{IndexStr:000}.azumolabitem");

            HttpResponseMessage httpResponseMessage = __HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).Result;
            BufferedStream readDownloadStream = new BufferedStream(httpResponseMessage.Content.ReadAsStreamAsync().Result, _DownloadCache.Length);
            BufferedStream writeFileStream = new BufferedStream(new FileStream(savePath, FileMode.OpenOrCreate), _DownloadCache.Length);

            int len;
            while ((len = readDownloadStream.Read(_DownloadCache)) > 0)
            {
                Span<byte> cache = _DownloadCache[..len];
                writeFileStream.Write(cache);
            }

            readDownloadStream.Close();
            writeFileStream.Close();
            readDownloadStream.Dispose();
            writeFileStream.Dispose();
        }

        #region 私有工具方法

        private void GetFileInfo(HttpResponseMessage responseMessage)
        {
            string filename = responseMessage.Content.Headers.ContentDisposition?.FileName! ?? string.Empty;

            Downloading.FileName = filename.Replace("\"", string.Empty);
            Downloading.FileSize = responseMessage.Content.Headers.ContentLength!.Value;
        }

        private HttpRequestMessage IsSupportRange(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Headers.Range = new RangeHeaderValue(0, 9);
            HttpResponseMessage httpResponseMessage = __HttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).Result;

            long FileLen = 0;
            ContentRangeHeaderValue rangeValue;
            if ((rangeValue = httpResponseMessage.Content.Headers.ContentRange!) != null && rangeValue.HasRange)
            {
                if (httpResponseMessage.Content.Headers.ContentRange!.Length.HasValue)
                    FileLen = httpResponseMessage.Content.Headers.ContentRange.Length.Value;
            }
            if (FileLen == 0)
            {
                throw new NoSupportRange();
            }
            Downloading.CanRange = true;
            return new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri);
        }

        #endregion
    }
}
