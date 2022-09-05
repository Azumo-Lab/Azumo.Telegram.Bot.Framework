//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Telegram.Bot.Framework.InternalFramework.FrameworkHelper
{
    internal static class ThrowHelper
    {
        public static void ThrowIfNullOrEmpty(string str)
        {
            ThrowIfNullOrEmpty(str, "文本不能为NULL，或空字符串");
        }

        public static void ThrowIfNullOrEmpty(string str, string ErrorInfo)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(ErrorInfo);
        }

        public static void ThrowIfZeroAndDown(int number, string ErrorInfo)
        {
            if (number <= 0)
                throw new ArgumentException(ErrorInfo);
        }

        public static void ThrowIfZeroAndDown(int number)
        {
            ThrowIfZeroAndDown(number, "不能小于或等于零");
        }

        public static void ThrowIfNull(object obj)
        {
            ThrowIfNull(obj, "参数不能为NULL");
        }

        public static void ThrowIfNull(object obj, string errorMsg)
        {
            if (obj == null)
                throw new ArgumentNullException(errorMsg);
        }
    }
}
