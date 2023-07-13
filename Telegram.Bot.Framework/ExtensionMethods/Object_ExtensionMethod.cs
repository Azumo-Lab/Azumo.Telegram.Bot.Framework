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

using System.Collections.Generic;
using System.IO;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public static class Object_ExtensionMethod
    {
        #region 字符串类型，处理，扩展方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return str.IsNull() || str.IsEmpty();
        }

        public static bool IsNullOrTrimEmpty(this string str)
        {
            return str.IsNull() || str.Trim().IsEmpty();
        }

        public static bool IsEmpty(this string str)
        {
            return str == string.Empty;
        }

        public static string GetVal(this string str, string defVal)
        {
            return str.IsNullOrEmpty() ? defVal : str;
        }

        #endregion

        #region Object类型的处理，扩展方法

        /// <summary>
        /// 判断对象是否是空
        /// </summary>
        /// <param name="obj">要判断的对象</param>
        /// <returns>布尔值，Null为True，反之</returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="objs">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull(params object[] objs)
        {
            if (objs.IsNull())
                return true;
            foreach (object item in objs)
            {
                if (item.IsNull())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="arrays">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull<T>(this T[] arrays)
        {
            return HasAnyNull(arrays);
        }

        /// <summary>
        /// 判断对象中是否有任意的空值
        /// </summary>
        /// <param name="list">要判断的对象</param>
        /// <returns>布尔值，数组有任意空元素为True，反之</returns>
        public static bool HasAnyNull<T>(this List<T> list)
        {
            if (list.IsNull())
                return true;
            foreach (T item in list)
            {
                if (item.IsNull())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断对象中的对象是否都是Null
        /// </summary>
        /// <param name="objs">要判断的对象</param>
        /// <returns>布尔值，数组所有元素皆为Null为True，反之</returns>
        public static bool HasAllNull(params object[] objs)
        {
            if (objs.IsNull())
                return true;
            foreach (object item in objs)
            {
                if (!item.IsNull())
                    return false;
            }
            return true;
        }

        #endregion

        #region 文件类的扩展处理
        public static bool IsFilePath(this string path)
        {
            return File.Exists(path);
        }
        #endregion

        #region 数组类型的处理

        public static T GetVal<T>(this T[] list, int index)
        {
            int newIndex = 0;
            for (int i = 0; i < index; i++)
            {
                newIndex++;
                if (newIndex >= list.Length)
                    newIndex = 0;
            }
            return list[newIndex];
        }

        public static bool IsEmpty<T>(this T[] list)
        {
            return list.IsNull() || list.Length == 0;
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.IsNull() || list.Count == 0;
        }

        #endregion
    }
}
