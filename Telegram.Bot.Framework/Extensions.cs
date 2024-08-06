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
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Params;

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
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
            IGetParamTypeList = typeof(IGetParam).GetAllSameType()
                .Where(x => Attribute.IsDefined(x, typeof(TypeForAttribute)))
                .Select(x => (x, (TypeForAttribute)Attribute.GetCustomAttribute(x, typeof(TypeForAttribute))!))
                .ToList();

            IGetParamTypeList ??=
#if NET8_0_OR_GREATER
            [];
#else
            new List<(Type classType, TypeForAttribute ForType)>();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        internal static IReadOnlyList<(Type classType, TypeForAttribute ForType)> IGetParamTypeList { get; }

        /// <summary>
        /// 程序全部的类型
        /// </summary>
        internal static IReadOnlyList<Type> AllTypes { get; }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileTypeEnum GetFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            var Image = new[] { ".JPG", ".JPEG", ".PNG", ".GIF", ".BMP", ".WEBP" };
            var Audio = new[] { ".MP3", ".WAV", ".OGG", ".M4A" };
            var Video = new[] { ".MP4", ".AVI", ".MKV", ".MOV", ".WMV" };

            var dic = new Dictionary<FileTypeEnum, string[]>
            {
                { FileTypeEnum.Image, Image },
                { FileTypeEnum.Audio, Audio },
                { FileTypeEnum.Video, Video }
            };

            if (string.IsNullOrEmpty(extension))
                return FileTypeEnum.File;

            foreach (var item in dic)
                if (item.Value.Contains(extension))
                    return item.Key;

            return FileTypeEnum.File;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FileTypeEnum
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 音频
        /// </summary>
        Audio,
        /// <summary>
        /// 视频
        /// </summary>
        Video,
        /// <summary>
        /// 文件
        /// </summary>
        File
    }
}