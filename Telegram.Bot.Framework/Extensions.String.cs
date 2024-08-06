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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static List<Type> GetAllTypeSameNameSpace(this string nameSpace) =>
            AllTypes.Where(x => x.Namespace == nameSpace).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static Stream OpenBufferedStream(this string filePath)
        {
            if(string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var bufferedSize = Consts.BUFFED_STREAM_CACHE_128KB;
            switch (GetFileType(filePath))
            {
                case FileTypeEnum.Image:
                    bufferedSize = Consts.BUFFED_STREAM_CACHE_256KB;
                    break;
                case FileTypeEnum.Audio:
                    bufferedSize = Consts.BUFFED_STREAM_CACHE_1MB;
                    break;
                case FileTypeEnum.Video:
                    bufferedSize = Consts.BUFFED_STREAM_CACHE_4MB;
                    break;
                case FileTypeEnum.File:
                    bufferedSize = Consts.BUFFED_STREAM_CACHE_256KB;
                    break;
            }

            return new BufferedStream(new FileStream(filePath, FileMode.Open), bufferedSize);
        }
    }
}
