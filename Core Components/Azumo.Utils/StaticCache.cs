using System;
using System.Collections.Generic;
using System.Text;

namespace Azumo.Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal static class StaticCache<K, V>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly static Dictionary<K, V> __Cache = new Dictionary<K, V>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCache(K key, V value)
        {
            __Cache.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool GetCache(K key, out V v)
        {
            return __Cache.TryGetValue(key, out v);
        }
    }
}
