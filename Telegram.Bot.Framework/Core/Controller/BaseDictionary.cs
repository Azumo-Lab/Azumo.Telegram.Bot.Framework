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

using System;
using System.Collections.Generic;

namespace Telegram.Bot.Framework.Core.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal class BaseDictionary<K, V> where K : notnull where V : class
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<K, V> _dictionary =
#if NET8_0_OR_GREATER
            [];
#else
            new Dictionary<K, V>();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(K key, V value) => _dictionary.TryAdd(key, value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(K key, V value)
        {
            if (!TryAdd(key, value))
                _dictionary[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V? Get(K key)
        {
            _ = TryGet(key, out var value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool TryGet(K key, out V? v) =>
            _dictionary.TryGetValue(key, out v);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public bool TryGet(K key, out V v, Func<V> defVal)
        {
            bool result;
            if (!(result = TryGet(key, out var outVal)))
                outVal = defVal();
            v = outVal!;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(K key) =>
            _dictionary.ContainsKey(key);
    }
}
