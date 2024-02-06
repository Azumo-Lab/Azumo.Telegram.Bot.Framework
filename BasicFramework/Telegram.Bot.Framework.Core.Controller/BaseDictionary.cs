//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

namespace Telegram.Bot.Framework.Core.Controller;
internal class BaseDictionary<K, V> where K : notnull
{
    private readonly Dictionary<K, V> _dictionary = [];

    public bool TryAdd(K key, V value) => _dictionary.TryAdd(key, value);

    public void AddOrUpdate(K key, V value)
    {
        if (!TryAdd(key, value))
            _dictionary[key] = value;
    }

    public V? Get(K key)
    {
        TryGet(key, out var value);
        return value;
    }

    public bool TryGet(K key, out V? v) => 
        _dictionary.TryGetValue(key, out v);

    public bool TryGet(K key, out V v, Func<V> defVal)
    {
        bool result;
        if (!(result = TryGet(key, out var outVal)))
            outVal = defVal();
        v = outVal!;
        return result;
    }

    public bool ContainsKey(K key) => 
        _dictionary.ContainsKey(key);
}
