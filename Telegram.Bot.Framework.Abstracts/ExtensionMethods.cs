using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstracts
{
    public static class ExtensionMethods
    {
        #region 扩展方法 String类型
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrTrimEmpty(this string str)
        {
            return IsNullOrEmpty(str) || IsNullOrEmpty(str.Trim());
        }

        public static bool NotNullOrEmpty(this string str)
        {
            return !IsNullOrEmpty(str);
        }

        public static bool NotNullOrTrimEmpty(this string str)
        {
            return !IsNullOrTrimEmpty(str);
        }

        public static string GetValue(this string str, string defVal = "")
        {
            if (str.IsNull())
                return defVal;
            return str;
        }

        public static string GetValue(this string str, ref string changeValue, string defVal = "")
        {
            if(str.IsNull())
            {
                changeValue = defVal;
                return changeValue;
            }
            return str;
        }
        #endregion

        #region 扩展方法 Object 类型
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool NotNull(this object obj)
        {
            return !IsNull(obj);
        }
        #endregion

        #region 扩展方法 ICollection<T> 类型
        public static bool IsEmpty<T>(this ICollection<T> ts)
        {
            return ts.IsNull() || ts.Count == 0;
        }

        #endregion
    }
}
