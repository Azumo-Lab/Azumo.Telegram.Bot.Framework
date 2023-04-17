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

using System;

namespace Telegram.Bot.Framework.Helper
{
    /// <summary>
    /// 这是 <see cref="string"/> 的扩展方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="string"/> 的扩展方法, 通常是以下的方式：
    /// </para>
    /// <code>
    /// public static void MethodName(this <see cref="string"/> obj)
    /// </code>
    /// </remarks>
    public static class StringHelper
    {
        #region 抛出异常

        /// <summary>
        /// 如果字符串为空的话，则会抛出异常
        /// </summary>
        /// <remarks>
        /// 这里的空的情况是：NULL的情况下。<br/>
        /// 如果想要在传入值为空字符串（<see cref="string.Empty"/>）的情况下抛出异常，请使用 <see cref="ThrowIfNullOrEmpty(string)"/>
        /// </remarks>
        /// <param name="str">传入字符串</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull(this string str)
        {
            if (str.IsNull())
                throw new ArgumentNullException(nameof(str));
        }

        /// <summary>
        /// 在传入值为空字符串（<see cref="string.Empty"/>）和 NULL 的情况下抛出异常
        /// </summary>
        /// <param name="str">传入字符串</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNullOrEmpty(this string str)
        {
            if (str.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(str));
        }
        #endregion

        #region 对String类型进行扩展

        /// <summary>
        /// 判断一个字符串是否是 NULL 或 <see cref="string.Empty"/>
        /// </summary>
        /// <param name="str">传入值</param>
        /// <returns>True：是空，False：非空</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断一个字符串是否是 NULL 或 <see cref="string.Empty"/>，如果不是，则调用 <see cref="string.Trim"/> 后判断
        /// </summary>
        /// <param name="str">传入值</param>
        /// <returns>True：是空，False：非空</returns>
        public static bool IsTrimEmpty(this string str)
        {
            return IsNullOrEmpty(str) || IsNullOrEmpty(str.Trim());
        }

        #endregion

        #region 获取String字符串的值

        /// <summary>
        /// 获取这个字符串的值
        /// </summary>
        /// <remarks>
        /// 这个方法可以安全的取得值，传入值是 NULL 或者 <see cref="string.Empty"/> 时，则使用默认值
        /// </remarks>
        /// <param name="str">传入值</param>
        /// <param name="defVal">默认值</param>
        /// <returns>字符串值</returns>
        public static string GetValue(this string str, string defVal)
        {
            return IsNullOrEmpty(str) ? defVal : str;
        }

        /// <summary>
        /// 从字符串中获取一个 <see cref="char"/> 类型的值
        /// </summary>
        /// <remarks>
        /// 注意：如果没有从字符串中取得值，则返回 <see cref="char.MinValue"/> 作为默认的返回值
        /// </remarks>
        /// <param name="str">传入值</param>
        /// <param name="index">座标</param>
        /// <returns><see cref="char"/> 返回的值</returns>
        public static char GetValue(this string str, int index)
        {
            return GetValue(str, index, char.MinValue);
        }

        /// <summary>
        /// 从字符串中获取一个 <see cref="char"/> 类型的值
        /// </summary>
        /// <param name="str">传入字符串</param>
        /// <param name="index">座标</param>
        /// <param name="defVal">默认的返回值</param>
        /// <returns><see cref="char"/> 返回的值</returns>
        public static char GetValue(this string str, int index, char defVal)
        {
            return str.IsNullOrEmpty() || index < 0 || index >= str.Length ? defVal : str[index];
        }

        #endregion

    }
}
