using System;
using System.Collections.Generic;

namespace Azumo.Utils
{
    /// <summary>
    /// 静态缓存
    /// </summary>
    /// <remarks>
    /// 每一个不同的类型的 <typeparamref name="K"/> 和 <typeparamref name="V"/> 都会生成一个新的缓存数据。
    /// 可以放心使用。
    /// </remarks>
    /// <typeparam name="K">缓存数据的Key值类型</typeparam>
    /// <typeparam name="V">要进行缓存的数据类型</typeparam>
    public static class StaticCache<K, V> where K : notnull
    {
        /// <summary>
        /// 缓存数据的字典
        /// </summary>
        private static readonly Dictionary<K, V> __Cache = [];

        /// <summary>
        /// 设置缓存的数据
        /// </summary>
        /// <param name="key">缓存数据的Key</param>
        /// <param name="value">要缓存的数据</param>
        public static void SetCache(K key, V value)
        {
            __Cache.Add(key, value);
        }

        /// <summary>
        /// 获取指定缓存的数据
        /// </summary>
        /// <param name="key">缓存数据的Key</param>
        /// <param name="v">要取得的缓存数据</param>
        /// <returns>返回是否能够成功取得数据</returns>
        public static bool GetCache(K key, out V? v)
        {
            return __Cache.TryGetValue(key, out v);
        }

        /// <summary>
        /// 获取指定的缓存数据
        /// </summary>
        /// <remarks>
        /// 获取指定的缓存数据，如果数据不存在，则调用后面的方法，来获取，获取成功后，会进行缓存。
        /// </remarks>
        /// <param name="key">缓存数据的Key</param>
        /// <param name="func">无法取得数据时的方法</param>
        /// <returns>返回取得的数据</returns>
        public static V GetCache(K key, Func<V> func)
        {
            if (!GetCache(key, out V? value))
            {
                ArgumentNullException.ThrowIfNull(func);

                value = func();
                SetCache(key, value);
            }
            return value!;
        }

        /// <summary>
        /// 移除指定的数据
        /// </summary>
        /// <param name="key">缓存数据的Key</param>
        /// <returns>返回是否成功删除数据</returns>
        public static bool Remove(K key)
        {
            return __Cache.Remove(key);
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public static void Clear()
        {
            __Cache.Clear();
        }
    }
}
