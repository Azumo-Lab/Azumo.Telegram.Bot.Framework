//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class ISession_ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            object obj;
            if ((obj = session.Get(key)) != null)
                try
                {
                    return (T)obj;
                }
                catch (Exception)
                {
                    return default;
                }
            return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="outObj"></param>
        /// <returns></returns>
        public static bool TryGet<T>(this ISession session, string key, out T outObj)
        {
            bool result = session.TryGetValue(key, out object obj);
            outObj = result ? (T)obj : default;
            return result;
        }
    }
}
