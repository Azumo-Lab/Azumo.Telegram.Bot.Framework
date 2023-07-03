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

        public static bool IsNull(this object obj)
        {
            return obj == null;
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

        #endregion
    }
}
