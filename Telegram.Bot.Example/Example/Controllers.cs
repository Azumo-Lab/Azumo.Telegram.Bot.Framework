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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Example.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class Controllers : TelegramController
    {
        [Command(nameof(Start), CommandInfo = "本条指令")]
        public async Task Start()
        {
            string message = "你好，这里是演示机器人，你可以通过以下的几个命令来测试机器人：";

            message += Environment.NewLine;
            message += Environment.NewLine;

            message += GetCommandInfosString();

            message += Environment.NewLine;
            message += "项目地址：https://github.com/Azumo-Lab/Telegram.Bot.Framework/";

            string logFile = "UsersInfo.log";
            if (!File.Exists(logFile))
                File.Create(logFile).Close();
            using (FileStream fs = new(logFile, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(TelegramContext.Update.Message.Chat));
                }
            }
            
            await SendTextMessage(message);
        }
    }
}
