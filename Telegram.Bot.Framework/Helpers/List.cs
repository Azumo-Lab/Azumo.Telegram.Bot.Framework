using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListDisposable<T> : List<T>, IDisposable where T : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            foreach (var item in this)
                item.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListAsyncDisposable<T> : List<T>, IAsyncDisposable where T : IAsyncDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            foreach (var item in this)
                await item.DisposeAsync();
        }
    }
}
