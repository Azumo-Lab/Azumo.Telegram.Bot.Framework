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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Storage;

namespace Telegram.Bot.Framework.InternalCore.Storage
{
    /// <summary>
    /// 一个临时存储实现
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ISession))]
    [DebuggerDisplay("Session:{ID}, Count:{Count}")]
    internal class SessionStorage : ISession
    {
        /// <summary>
        /// 保存字典
        /// </summary>
        private readonly ConcurrentDictionary<object, object> _cache =
#if NET8_0_OR_GREATER
            [];
#else
            new ConcurrentDictionary<object, object>();
#endif

        /// <summary>
        /// 这个临时存储的ID
        /// </summary>
        public string ID { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 存储的总数
        /// </summary>
        public int Count => _cache.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(object key, object value) => _cache.TryAdd(key, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(object key, object value)
        {
            if (!_cache.TryAdd(key, value))
                _cache[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear() => _cache.Clear();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            _ = _cache.TryGetValue(key, out var obj);
            return obj!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key) => _cache.Remove(key, out _);
    }
}
