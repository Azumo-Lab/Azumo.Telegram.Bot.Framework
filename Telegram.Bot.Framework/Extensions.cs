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

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        /// 图片的文件类型
        /// </summary>
        private static readonly string[] Image;

        /// <summary>
        /// 音频的文件类型
        /// </summary>
        private static readonly string[] Audio;

        /// <summary>
        /// 视频的文件类型
        /// </summary>
        private static readonly string[] Video;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <remarks>
        /// 对必要的内容进行初始化
        /// </remarks>
        static Extensions()
        {
            const string JPG = ".JPG";
            const string JPEG = ".JPEG";
            const string PNG = ".PNG";
            const string GIF = ".GIF";
            const string BMP = ".BMP";
            const string WEBP = ".WEBP";
            const string MP3 = ".MP3";
            const string WAV = ".WAV";
            const string OGG = ".OGG";
            const string M4A = ".M4A";
            const string MP4 = ".MP4";
            const string AVI = ".AVI";
            const string MKV = ".MKV";
            const string MOV = ".MOV";
            const string WMV = ".WMV";
            // 初始化各个类型
            // 首先从配置文件中读取，如果没有则使用默认值
            IConfigurationSection fileType;
            if (TelegramBot.MainConf == null || !(fileType = TelegramBot.MainConf.GetSection("FileType")).Exists())
            {
#if NET8_0_OR_GREATER
                Image = [JPG, JPEG, PNG, GIF, BMP, WEBP];
                Audio = [MP3, WAV, OGG, M4A];
                Video = [MP4, AVI, MKV, MOV, WMV];
#else
                Image = new[] { JPG, JPEG, PNG, GIF, BMP, WEBP };
                Audio = new[] { MP3, WAV, OGG, M4A };
                Video = new[] { MP4, AVI, MKV, MOV, WMV };
#endif
            }
            else
            {
                // 读取配置文件
                // 如果配置文件中有Path字段则读取Path字段中的配置文件
                IConfigurationSection filePath;
                if ((filePath = fileType.GetSection("Path")).Exists())
                    fileType = new ConfigurationBuilder().AddJsonFile(filePath.Value!).Build().GetSection("FileType");
                Image = fileType.GetSection(nameof(Image)).GetChildren().Select(x => x.Value?.ToUpper()).Where(x => !string.IsNullOrEmpty(x)).ToArray()!;
                Audio = fileType.GetSection(nameof(Audio)).GetChildren().Select(x => x.Value?.ToUpper()).Where(x => !string.IsNullOrEmpty(x)).ToArray()!;
                Video = fileType.GetSection(nameof(Video)).GetChildren().Select(x => x.Value?.ToUpper()).Where(x => !string.IsNullOrEmpty(x)).ToArray()!;
            } 

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
        /// 创建回调函数的Key值
        /// </summary>
        /// <remarks>
        /// 回调函数使用这个Key值来进行识别
        /// </remarks>
        /// <returns>返回Key值</returns>
        internal static string CreateCallBackHash()
        {
            var guid = Guid.NewGuid().ToByteArray();
#if NET8_0_OR_GREATER
            guid = MD5.HashData(guid);
#else
            using (var md5 = MD5.Create())
            {
                guid = md5.ComputeHash(guid);
            }
#endif
            var result = guid.ByteToString();
            return $"c{result}";
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <remarks>
        /// 通过文件的扩展名来判断文件的类型
        /// </remarks>
        /// <param name="fileName">文件的名称</param>
        /// <returns><see cref="FileTypeEnum"/> 文件的类型</returns>
        public static FileTypeEnum GetFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

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

        internal static string ByteToString(this byte[] bytes) =>
#if NET8_0_OR_GREATER
            Convert.ToHexString(bytes);
#else
            BitConverter.ToString(bytes).Replace("-", string.Empty);
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="souceStr"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetHash(string souceStr, int time = 765)
        {
            if (string.IsNullOrEmpty(souceStr))
                return string.Empty;

            static string ProcesssHashString(string input)
            {
                var first = input.FirstOrDefault();
                if (first != char.MinValue)
                {
                RESWITCH:
                    switch (first)
                    {
                        case '0':
                            input += "{6720DA7E-1FFF-4463-B92D-24EA0C609A0B}";
                            break;
                        case '1':
                            input += "{99D12C9C-B058-4B08-A805-982690EFA6A2}";
                            break;
                        case '2':
                            input += "{B5842B21-9273-4C8B-BD28-0974389E31CA}";
                            break;
                        case '3':
                            input += "{E73000E6-448B-411B-A067-8E3B937E30B4}";
                            break;
                        case '4':
                            input += "{FB221488-7EA7-44A8-BBCF-3064A9057D0D}";
                            break;
                        case '5':
                            input += "{31995C55-F3BB-4300-A726-1641D810EE9D}";
                            break;
                        case '6':
                            input += "{94FD425C-2F53-4595-AEC5-45629E848B14}";
                            break;
                        case '7':
                            input += "{2EC8F48A-9D43-4BAA-80FA-3F67E572CB94}";
                            break;
                        case '8':
                            input += "{24D64E02-2358-45E2-A6DA-E536B3D0B782}";
                            break;
                        case '9':
                            input += "{73F75073-BEF4-4658-BA80-EF27AD3BEB25}";
                            break;
                        default:
                            input += "{6794A760-B73F-4AC8-AB22-76578CDA4AD4}";
                            first = ((int)first).ToString().First();
                            goto RESWITCH;
                    }
                    return input;
                }
                return string.Empty;
            }

            var hash = Encoding.UTF8.GetBytes(ProcesssHashString(souceStr));
#if NET8_0_OR_GREATER
            for (var i = 0; i < time; i++)
            {
                var timeStr = ByteToString(hash);
                hash = Encoding.UTF8.GetBytes(ProcesssHashString(timeStr));
                hash = SHA256.HashData(hash);
                hash = MD5.HashData(hash);
            }
#else
            using (HashAlgorithm Sha256 = SHA256.Create(), Md5 = MD5.Create())
            {
                for (var i = 0; i < time; i++)
                {
                    var timeStr = ByteToString(hash);
                    hash = Encoding.UTF8.GetBytes(ProcesssHashString(timeStr));
                    hash = Sha256.ComputeHash(hash);
                    hash = Md5.ComputeHash(hash);
                }
            }
#endif
            return ByteToString(hash);
        }
    }

    /// <summary>
    /// 文件的类型
    /// </summary>
    public enum FileTypeEnum
    {
        /// <summary>
        /// 图片类型文件
        /// </summary>
        Image,
        /// <summary>
        /// 音频类型文件
        /// </summary>
        Audio,
        /// <summary>
        /// 视频类型文件
        /// </summary>
        Video,
        /// <summary>
        /// 文件类型
        /// </summary>
        File,
    }
}