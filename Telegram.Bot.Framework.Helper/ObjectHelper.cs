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
    /// 这是 <see cref="object"/> 的扩展方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="object"/> 的扩展方法, 通常是以下的方式：
    /// </para>
    /// <code>
    /// public static void MethodName(this <see cref="object"/> obj)
    /// </code>
    /// </remarks>
    public static class ObjectHelper
    {
        #region 私有静态变量

        /// <summary>
        /// 简单缓存一下，所有的类型
        /// </summary>
        /// <remarks>
        /// 赋值与使用：<see cref="GetAllTypes"/> <br/>
        /// 清除缓存 <see cref="ClearCache"/>
        /// </remarks>
        private static readonly List<Type> __AllTypes = new List<Type>();

        /// <summary>
        /// 简单缓存一下数据
        /// </summary>
        /// <remarks>
        /// 赋值与使用 <see cref="GetSameType(object)"/> <br/>
        /// 清除缓存 <see cref="ClearCache"/>
        /// </remarks>
        private static readonly Dictionary<Type, List<Type>> __SameTypesDic = new Dictionary<Type, List<Type>>();
        #endregion

        #region 抛出异常

        /// <summary>
        /// 检测传入的对象是否为NULL，如果为空，则抛出异常
        /// </summary>
        /// <remarks>
        /// 如果传入的对象为NULL，则抛出异常： <see cref="ArgumentNullException"/>
        /// </remarks>
        /// <param name="obj">传入的检测对象</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull(this object obj)
        {
            if (obj.IsNull())
                throw new ArgumentNullException(nameof(obj));
        }
        #endregion

        #region 判断对象是否为空

        /// <summary>
        /// 判断一个对象是否为NULL
        /// </summary>
        /// <param name="obj">需要判断的对象</param>
        /// <returns>如果为NULL，则返回True，反之</returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断多个对象是否全部为NULL
        /// </summary>
        /// <param name="objs">需要判断的对象</param>
        /// <returns>如果都为NULL，则返回True，任意不为NULL，则返回False</returns>
        public static bool HasAllNull(params object[] objs)
        {
            return objs.IsEmpty() || !objs.Where(o => !o.IsNull()).Any();
        }

        /// <summary>
        /// 判断多个对象是否任意一个对象为NULL
        /// </summary>
        /// <param name="objs">需要判断的对象</param>
        /// <returns>如果任意为NULL，则返回True，如果都不为NULL，则返回False</returns>
        public static bool HasAnyNull(params object[] objs)
        {
            if (objs.IsEmpty())
                return true;
            foreach (object obj in objs)
                if (obj.IsNull())
                    return true;
            return false;
        }
        #endregion

        #region 反射相关的操作

        /// <summary>
        /// 获得与传入类型相同的 <see cref="Type"/> 类型
        /// </summary>
        /// <remarks>
        /// <para>
        /// 通过这种方式取得的类型是可实例化的对象，例如，传入接口类型，取得的数据是 所有实现这个接口的类。<br/>
        /// 再例如，传入一个抽象类的情况，取得的数据则是 实现这个抽象类的类。
        /// </para>
        /// <para>
        /// 该方法具有缓存功能，可能会因为外部动态加载造成错误的执行结果，所以，如果有外部动态加载的话，在加载之后需要调用 <see cref="ClearCache"/> 清除缓存
        /// </para>
        /// <para>
        /// 传入数据类型：<br/>
        /// <paramref name="obj"/> : 可为：<see cref="Type"/> 类型，也可以是 <see cref="object"/> 类型
        /// </para>
        /// <para>
        /// 例如：
        /// <code>
        ///     typeof(IType).GetSameType();
        ///     (obj as IType).GetSameType();
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns>获得与传入类型相同的 <see cref="List{T}"/> 类型</returns>
        public static List<Type> GetSameType(this object obj)
        {
            Type? baseType = null;

            if (obj.IsNull())
                return Array.Empty<Type>().ToList();
            baseType = obj is Type type ? type : obj.GetType();
            if (__SameTypesDic.TryGetValue(baseType, out List<Type> typeList))
            {
                return typeList;
            }

            List<Type> allTypes = GetAllTypes();
            allTypes = allTypes.Where(x =>
            {
                return baseType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface;
            }).ToList();

            __SameTypesDic.TryAdd(baseType, allTypes);

            return allTypes;
        }

        /// <summary>
        /// 获取所有的类型
        /// </summary>
        /// <remarks>
        /// 通过这个方法，可以获取到当前运行程序加载的所有类型
        /// <para>
        /// 该方法具有缓存功能，可能会因为外部动态加载造成错误的执行结果，所以，如果有外部动态加载的话，在加载之后需要调用 <see cref="ClearCache"/> 清除缓存
        /// </para>
        /// </remarks>
        /// <returns>所有类型的集合 <see cref="List{T}"/></returns>
        public static List<Type> GetAllTypes()
        {
            // 如果没有，就简单缓存一下
            if (!__AllTypes.Any())
                __AllTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()));
            return __AllTypes;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <remarks>
        /// 当外部动态加载DLL之后，本程序的缓存可能不能及时清除更新，需要调用该方法手动清除。
        /// </remarks>
        public static void ClearCache()
        {
            __AllTypes.Clear();
            __SameTypesDic.Clear();
        }
        #endregion
    }
}
