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
            message += GetCommandInfosString();

            await SendTextMessage(message);
        }

        [Command(nameof(NowTime), CommandInfo = "输出现在的时间")]
        public async Task NowTime()
        {
            await SendTextMessage(DateTime.Now.ToString("现在的时间是 yyyy 年 MM 月 dd 日 HH 点 mm 分 ss 秒"));
        }
    }
}
