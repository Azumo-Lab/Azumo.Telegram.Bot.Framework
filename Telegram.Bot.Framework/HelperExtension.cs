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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class HelperExtension
    {
        #region IsEmpty()

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            if (list == null)
                return true;
            return !list.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return str == null || str.Length == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTrimEmpty(this string str)
        {
            return !IsEmpty(str) && str.Trim().IsEmpty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static bool IsEmpty(this FileInfo fileInfo)
        {
            if (fileInfo == null)
                return true;
            return fileInfo.Length == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                return true;
            return !directoryInfo.GetDirectories().Any() && !directoryInfo.GetFiles().Any();
        }
        #endregion

        #region IServiceProvider
        public static TelegramContext GetTelegramContext(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<TelegramContext>();
        }
        #endregion
    }
}
