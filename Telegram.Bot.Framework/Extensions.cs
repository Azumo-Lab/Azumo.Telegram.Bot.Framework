//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 初始化
        /// </summary>
        static Extensions()
        {
            IGetParamTypeList = typeof(IGetParam).GetAllSameType()
                .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
                .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
                .ToList();

            IGetParamTypeList ??=
#if NET8_0_OR_GREATER
            [];
#elif NETSTANDARD2_1_OR_GREATER
            new List<(Type classType, TypeForAttribute ForType)>();
#endif
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        private static IReadOnlyList<(Type classType, TypeForAttribute ForType)> IGetParamTypeList { get; }

        /// <summary>
        /// 程序全部的类型
        /// </summary>
        public static IReadOnlyList<Type> AllTypes { get; }
    }
}