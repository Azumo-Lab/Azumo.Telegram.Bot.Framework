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
using System.IO;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 文件重新命名
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="newName"></param>
        public static void Rename(this FileInfo fileInfo, string newName)
        {
            var newPath = (fileInfo.DirectoryName ?? string.Empty) + newName;
            fileInfo.MoveTo(newPath);
        }

        /// <summary>
        /// 文件重新命名
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="rename"></param>
        public static void Rename(this FileInfo fileInfo, Func<string, string, string, string> rename)
        {
            var directoryName = fileInfo.DirectoryName ?? string.Empty;
            var oldName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            var exName = fileInfo.Extension;
            var newPath = rename(directoryName, oldName, exName);
            fileInfo.Rename(newPath);
        }

        /// <summary>
        /// 变更文件扩展名
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="newExtension"></param>
        public static void ChangeExtension(this FileInfo fileInfo, string newExtension) =>
            Rename(fileInfo, (directoryName, oldName, exName) => Path.Combine(directoryName, oldName + newExtension));

        /// <summary>
        /// 追加文件扩展名
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="extensionName"></param>
        public static void AppendExtension(this FileInfo fileInfo, string extensionName) =>
            Rename(fileInfo, (directoryName, oldName, exName) => Path.Combine(directoryName, oldName + exName + extensionName));
    }
}
