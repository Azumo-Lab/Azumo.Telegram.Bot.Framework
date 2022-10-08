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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.TelegramControllerEX
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TelegramControllerPartial
    {
        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual async Task SendFile(byte[] File, string FileName)
        {

        }

        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual async Task SendFile(string FilePath)
        {
            if (File.Exists(FilePath))
                await SendFile(File.ReadAllBytes(FilePath), Path.GetFileName(FilePath));
            else
                throw new FileNotFoundException($"未找到文件 {FilePath}");
        }
    }
}
