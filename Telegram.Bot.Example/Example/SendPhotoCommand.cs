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

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Example.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class SendPhotoCommand : TelegramController
    {
        [Command(nameof(iCloudPhotoRandom), CommandInfo = "随机发送一张iCloud图片库中的图片")]
#pragma warning disable IDE1006 // 命名样式
        public async Task iCloudPhotoRandom()
#pragma warning restore IDE1006 // 命名样式
        {
            var Secrets = new ConfigurationBuilder().AddUserSecrets("98def42c-77dc-41cb-abf6-2c402535f4cb").Build();

            var files = Directory.GetFiles(Secrets.GetSection("iCloudPhotoPath").Value);
            await SendTextMessage("请稍后...正在发送...");
            await SendPhoto(files[Random(0, files.Length)], "这是一张随机的照片");
        }
    }
}
