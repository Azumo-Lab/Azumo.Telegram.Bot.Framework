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
        [Command(nameof(Start))]
        public async Task Start()
        {
            await SendTextMessage(
@"你好，这里是演示机器人，你可以通过以下的几个命令来测试机器人：
/Start      本指令
/Test       测试指令，一个参数
/Test2      测试指令，两个参数
/SayHello   让机器人输出 Hello World
/NowTime    让机器人输出现在的时间
");
        }

        [Command(nameof(NowTime))]
        public async Task NowTime()
        {
            await SendTextMessage(DateTime.Now.ToString("现在的时间是 yyyy 年 MM 月 dd 日 HH 点 mm 分 ss 秒"));
        }
    }
}
