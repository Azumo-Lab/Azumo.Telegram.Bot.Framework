using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Utils
{
    public static class Dictionary_ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ReplaceAdd<K, V>(this Dictionary<K, V> dic, K key, V value) where K : notnull
        {
            if (!dic.TryAdd(key, value))
                dic[key] = value;
        }
    }
}
