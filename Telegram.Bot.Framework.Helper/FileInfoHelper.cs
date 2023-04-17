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

using System.IO;

namespace Telegram.Bot.Framework.Helper
{
    /// <summary>
    /// 这是 <see cref="FileInfo"/> 的扩展方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="FileInfo"/> 的扩展方法, 通常是以下的方式：
    /// </para>
    /// <code>
    /// public static void MethodName(this <see cref="FileInfo"/> obj)
    /// </code>
    /// </remarks>
    public static class FileInfoHelper
    {
        /// <summary>
        /// 判断一个文件是否是 0 字节的空文件
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>True：空文件， False：非空文件</returns>
        public static bool IsEmpty(this FileInfo fileInfo)
        {
            return fileInfo.IsNull() || !fileInfo.Exists || fileInfo.Length == 0;
        }
    }
}
