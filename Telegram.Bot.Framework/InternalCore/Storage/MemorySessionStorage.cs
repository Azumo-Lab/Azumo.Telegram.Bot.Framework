//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Collections.Concurrent;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework.InternalCore.Storage;

/// <summary>
/// 
/// </summary>
/// <param name="memoryCache"></param>
internal class MemorySessionStorage(IMemoryCache memoryCache) : ISession
{
    private readonly IMemoryCache memoryCache = memoryCache;

    private readonly ConcurrentQueue<object> queue = new();

    public string ID { get; } = Guid.NewGuid().ToString();

    public int Count => queue.Count;

    public void Add(object key, object value)
    {
        queue.Enqueue(key);
        _ = memoryCache.Set(key, value);
    }

    public void AddOrUpdate(object key, object value) => Add(key, value);
    public void Clear()
    {
        foreach (var item in queue)
            memoryCache.Remove(item);

        queue.Clear();
    }
    public void Dispose() => Clear();
    public object Get(object key) => memoryCache.Get(key)!;
    public void Remove(object key) => memoryCache.Remove(key);
}
