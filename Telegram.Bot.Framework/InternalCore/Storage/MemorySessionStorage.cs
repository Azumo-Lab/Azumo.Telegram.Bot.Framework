//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Telegram.Bot.Framework.Storage;

namespace Telegram.Bot.Framework.InternalCore.Storage
{
    /// <summary>
    /// 
    /// </summary>
    internal class MemorySessionStorage : ISession
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public MemorySessionStorage(IMemoryCache memoryCache) =>
            this.memoryCache = memoryCache;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// 
        /// </summary>
        private readonly HashSet<object> queue =
#if NET8_0_OR_GREATER
            [];
#else
            new HashSet<object>();
#endif

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        public int Count => queue.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(object key, object value)
        {
            _ = queue.Add(key);
            _ = memoryCache.Set(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(object key, object value) => Add(key, value);

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var item in queue)
                memoryCache.Remove(item);

            queue.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key) => memoryCache.Get(key)!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key) => memoryCache.Remove(key);
    }
}
