using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helper;

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
                    return args.GetValue(i + 1, string.Empty);
            }
            return string.Empty;
        }
    }
}
