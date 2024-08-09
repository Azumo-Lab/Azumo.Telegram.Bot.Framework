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
//
//  Author: 牛奶

using System;

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// 异常帮助类
    /// </summary>
    /// <remarks>
    /// 用于抛出异常，实用于各类框架
    /// </remarks>
#if NET8_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:使用表达式主体来表示方法", Justification = "<挂起>")]
#endif
    internal static class ExceptionHelper
    {
        /// <summary>
        /// 字符串为空或null时抛出异常
        /// </summary>
        /// <param name="value">要检查的字符串</param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNullOrEmpty(string? value, string paramName)
        {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrEmpty(value, paramName);
#elif NETSTANDARD2_1_OR_GREATER
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(paramName);
#endif
        }

        /// <summary>
        /// 当参数为null时抛出异常
        /// </summary>
        /// <param name="value">要检查的参数</param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull(object? value, string paramName)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(value, paramName);
#elif NETSTANDARD2_1_OR_GREATER
            if (value == null)
                throw new ArgumentNullException(paramName);
#endif
        }
    }
}
