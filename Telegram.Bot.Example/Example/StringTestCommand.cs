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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Example.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class StringTestCommand : TelegramController
    {
        [DefaultMessageType(Types.Enums.MessageType.Text)]
        public async Task StringCatch()
        {
            await SendTextMessage("保底消息");
        }

        [DefaultMessageType(Types.Enums.MessageType.Photo)]
        public async Task PhotoCatch()
        {
            var photoSize = Context.Update.Message.Photo.ToList().OrderBy(x => x.FileSize).LastOrDefault();
            Directory.CreateDirectory("TESTPIC");
            using (StreamWriter sw = new StreamWriter(new FileStream($"TESTPIC/{photoSize.FileId}", FileMode.OpenOrCreate)))
            {
                sw.Write(JsonConvert.SerializeObject(photoSize));
            }
            await SendTextMessage("已保存");
            await SendTextMessage(
@"
你可以使用 /GetAllSendPhoto 获取全部图片，
也可以使用 /GetSendPhoto 随机获取一张图片");
        }
    }
}
