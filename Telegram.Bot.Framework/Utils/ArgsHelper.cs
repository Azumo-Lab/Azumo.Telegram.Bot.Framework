using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Utils
{
    /// <summary>
    /// 帮助创建收集参数的工具类
    /// </summary>
    public static class ArgsHelper
    {
        /// <summary>
        /// 获取参数， -Token "Token" -Proxy "http://127.0.0.1:7890/"
        /// </summary>
        /// <param name="args">参数列表</param>
        /// <param name="name">想要捕获的参数</param>
        /// <returns>返回的是想要的参数</returns>
        public static string GetArgs(this string[] args, string name)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower() == name.ToLower() || arg.ToUpper() == name.ToUpper())
                    return GetVal(args, i + 1, null);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="Index"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static T GetVal<T>(this IEnumerable<T> values, int Index, T defVal)
        {
            if (values == null)
                return defVal;
            if (Index >= values.Count())
                return default;
            return values.ToArray()[Index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static T GetVal<T>(this IEnumerable<T> values, int Index)
        {
            return GetVal(values, Index, default);
        }
    }
}
