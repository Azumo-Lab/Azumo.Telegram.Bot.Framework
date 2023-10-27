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

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static IReadOnlyList<Type> AllTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        static ReflectionHelper()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Type> FindTypeOf(this object obj)
        {
            Type type = obj is Type newType ? newType : obj.GetType();

            return Cache(CacheKey(type.FullName, nameof(FindTypeOf)), () => AllTypes.Where(type.IsAssignableFrom).Where(x => !x.IsInterface && !x.IsAbstract).ToList()) ?? new List<Type>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> FindAttribute<T>(this Type type) where T : Attribute
        {
            return Cache(CacheKey(type.FullName, nameof(FindAttribute)), () => Attribute.GetCustomAttributes(type, typeof(T)).Select(x => (T)x).ToList()) ?? new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> FindAttribute<T>(this object type) where T : Attribute
        {
            return FindAttribute<T>(type.GetType());
        }

        #region 保存缓存

        private static readonly Dictionary<string, object> __CacheObjDic = new();

        private static string CacheKey(string typeFullName, string methodName)
        {
            return $"{typeFullName}.{methodName}";
        }

        private static T Cache<T>(string cacheKey, Func<T> cacheObj)
        {
            T result = default;
            if (!__CacheObjDic.TryGetValue(cacheKey, out object obj))
            {
                result = cacheObj();
                _ = __CacheObjDic.TryAdd(cacheKey, result);
            }
            else
            {
                try
                {
                    result = (T)obj;
                }
                catch (Exception)
                { }
            }
            return result;
        }

        #endregion

    }
}
