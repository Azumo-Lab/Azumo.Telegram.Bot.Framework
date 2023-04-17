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
using System.Collections.Generic;
using System.Linq;

namespace Telegram.Bot.Framework.Helper
{
    /// <summary>
    /// 这是 <see cref="IEnumerable{T}"/> 的扩展方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IEnumerable{T}"/> 的扩展方法, 通常是以下的方式：
    /// </para>
    /// <code>
    /// public static void MethodName(this <see cref="IEnumerable{T}"/> obj)
    /// </code>
    /// </remarks>
    public static class IEnumerableHelper
    {
        #region 抛出异常

        /// <summary>
        /// 如果一个 <see cref="IEnumerable{T}"/> 类型是NULL，或者其中没有值，则会抛出异常
        /// </summary>
        /// <typeparam name="T">传入的泛型</typeparam>
        /// <param name="values">传入值</param>
        /// <exception cref="ArgumentNullException">参数为空的异常</exception>
        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> values)
        {
            if (values.IsEmpty())
                throw new ArgumentNullException(nameof(values));
        }

        #endregion

        /// <summary>
        /// 判断一个传入值是否是NULL，或者是一个长度为0的集合
        /// </summary>
        /// <typeparam name="T">传入泛型</typeparam>
        /// <param name="values">传入值</param>
        /// <returns>True：是空，False：非空</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> values)
        {
            return values.IsNull() || !values.Any();
        }

        /// <summary>
        /// 从传入的集合中获取值
        /// </summary>
        /// <remarks>
        /// 从集合中取得一个指定Index的值，如果没有这个值（Index过大，超过集合数量），则会返回一个默认值
        /// </remarks>
        /// <typeparam name="T">传入泛型</typeparam>
        /// <param name="values">传入值</param>
        /// <param name="index">取得值的Index</param>
        /// <param name="defVal">默认值</param>
        /// <returns>取得值或者默认值</returns>
        public static T GetValue<T>(this IEnumerable<T> values, int index, T defVal)
        {
            return IsEmpty(values) || values.Count() <= index ? defVal : values.Skip(index).FirstOrDefault();
        }
    }
}
