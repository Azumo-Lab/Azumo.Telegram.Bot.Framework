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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Users
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, typeof(ISession))]
    internal class Session : ISession
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, object> __Val = new();

        public string SessionID { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return TryGetValue(key, out object value) ? value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasVal(string key)
        {
            return __Val.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return __Val.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            _ = __Val.TryAdd(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return __Val.TryGetValue(key, out value);
        }
    }
}
