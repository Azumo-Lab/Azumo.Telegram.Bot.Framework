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

using System;
using System.Collections.Generic;
using System.IO;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public class FileStitching
    {

        private readonly BufferedStream __FileStream;
        private const int CACHE_SIZE = 1024;
        private readonly List<string> __PathList = new();

        public FileStitching(string filePath, bool OverWrite = false)
        {
            __FileStream = File.Exists(filePath) && OverWrite
                ? new BufferedStream(new FileStream(filePath, FileMode.OpenOrCreate), CACHE_SIZE)
                : new BufferedStream(new FileStream(filePath, FileMode.Append), CACHE_SIZE);
        }

        public FileStitching AddFile(string path)
        {
            __PathList.Add(path);
            return this;
        }

        public FileStitching AddFile(string path, int Index)
        {
            if (Index >= __PathList.Count)
            {
                return AddFile(path);
            }
            else
            {
                __PathList.Insert(Index, path);
            }
            return this;
        }

        public FileStitching AddFile(Stream stream)
        {
            using (BufferedStream memory = new(stream, CACHE_SIZE))
            {
                Span<byte> readBytes = stackalloc byte[CACHE_SIZE];
                int readSize;
                while ((readSize = memory.Read(readBytes)) != 0)
                {
                    readBytes = readBytes[..readSize];
                    _ = AddFile(readBytes);
                }
            }
            return this;
        }

        public FileStitching AddFile(Span<byte> datas)
        {
            __FileStream.Write(datas);
            return this;
        }
    }
}
