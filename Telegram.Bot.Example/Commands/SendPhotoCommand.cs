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

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Example.Makers;
using Telegram.Bot.Example.Proc;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;
using static System.Net.WebRequestMethods;

namespace Telegram.Bot.Example.Commands
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
            await Context.SendTextMessage("请稍后...正在发送...");
            await Context.SendPhoto(files[RandomMethod.RandomInt(0, files.Length)], "这是一张随机的照片");
        }

        [Command(nameof(SendMyPhoto), CommandInfo = "向机器人发送一张图片")]
        public async Task SendMyPhoto([Param("请发送你珍藏的图片：", CustomParamMaker = typeof(MyPhotoParamMaker))] PhotoSize photoSize)
        {
            Directory.CreateDirectory("TESTPIC");
            using (StreamWriter sw = new StreamWriter(new FileStream($"TESTPIC/{photoSize.FileId}", FileMode.OpenOrCreate)))
                sw.Write(JsonConvert.SerializeObject(photoSize));
            await Context.SendTextMessage("已经接收");
        }

        [Command(nameof(GetSendPhoto), CommandInfo = "从机器人那里随机获取一张图片")]
        public async Task GetSendPhoto()
        {
            var photos = Directory.GetFiles("TESTPIC");

            var file = photos[RandomMethod.RandomInt(0, photos.Length)];

            PhotoSize onephoto = JsonConvert.DeserializeObject<PhotoSize>(System.IO.File.ReadAllText(file));

            await Context.SendPhoto(onephoto, $"这是一张其他人传给Bot的图片，接收时间是：{new FileInfo(file).LastWriteTime}");
        }

        [Authentication(AuthenticationRole.BotAdmin)]
        [Command(nameof(GetAllSendPhoto), CommandInfo = "获取全部接收到的图片，这会发送大量图片")]
        public async Task GetAllSendPhoto([Param("此操作可能会发送大量图片，是否确定？(确定/取消)", CustomParamMaker = typeof(MyBoolParamMaker))] bool Yes)
        {
            if (Yes)
            {
                var photos = Directory.GetFiles("TESTPIC");
                foreach (var item in photos)
                {
                    PhotoSize onephoto = JsonConvert.DeserializeObject<PhotoSize>(System.IO.File.ReadAllText(item));
                    await Context.SendPhoto(onephoto, $"这是一张其他人传给Bot的图片，接收时间是：{new FileInfo(item).LastWriteTime}");
                }
            }
        }
    }
}
