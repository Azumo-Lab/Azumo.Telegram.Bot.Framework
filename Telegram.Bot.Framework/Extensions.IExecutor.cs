using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller;

namespace Telegram.Bot.Framework
{
    public static partial class Extensions
    {
        internal const string Auth = "{4468058F-8620-4ED4-A9A4-9092688B2C32}";
        internal const string AAA = "{126747C8-A049-4D03-85F1-428F3B688E42}";
        internal const string BBB = "{9C224CA9-D866-41A6-A403-B6A16CFC9218}";
        internal const string CCC = "{B6224B9F-F949-4576-A4FF-9574CE38F2F4}";
        internal const string DDD = "{E31AF0B4-C3CF-4683-89FB-8B61FB3EE564}";
        internal const string EEE = "{4BC3C3B7-32AC-408D-8CB6-075F6FEF0C7E}";
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="executor"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        internal static T GetOrCache<T>(this IExecutor executor, string key, Func<T> factory)
        {
            if (executor.Cache.TryGetValue(key, out var value))
                return (T)value;

            var result = factory();
            executor.Cache.Add(key, result!);
            return result;
        }
    }
}
