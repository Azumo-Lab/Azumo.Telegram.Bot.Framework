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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Example.Makers;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Example.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ParamTestCommand : TelegramController
    {
        [Command(nameof(Test1), CommandInfo = "传递一个参数，询问并捕获一个参数")]
        public async Task Test1([Param("请输入要说的话：")] string Message)
        {
            await Context.SendTextMessage($"你好，你要说的话是：{Message}");
            await Context.SendTextMessage($"这个测试例是方法中有一个参数并获取一个参数的值");
        }

        [Command(nameof(Test2), CommandInfo = "传递两个参数，询问并捕获两个参数")]
        public async Task Test2([Param("请输入第一句话：")] string FirstMessage, [Param("请输入第二句话：")] string TwoMessage)
        {
            await Context.SendTextMessage($"你说的第一句是：{FirstMessage}，第二句话是：{TwoMessage}");
            await Context.SendTextMessage($"这个测试例是方法中有两个参数并获取两个参数的值");
        }

        [Command(nameof(NowTime), CommandInfo = "输出现在的时间")]
        public async Task NowTime()
        {
            await Context.SendTextMessage(DateTime.Now.ToString("现在的时间是 yyyy 年 MM 月 dd 日 HH 点 mm 分 ss 秒"));
            await Context.SendTextMessage($"这个测试例是用来测试方法中没有参数的情况");
        }

        [Command("HelloAway", CommandInfo = "自定义的Message类型和参数获取类型，始终输出Hello")]
        public async Task ParamTest01(
            [Param("Test",
                CustomMessageType = typeof(MyStringParamMessage),
                CustomParamMaker = typeof(MyStringParamMaker))]
            string str
            )
        {
            await Context.SendTextMessage($"这是一个演示，始终输出Hello：{str}");
            await Context.SendTextMessage("这个演示方法有一个参数，参数使用了自定义的捕获方法，无论输入什么，始终入参：“Hello”");
        }
    }
}
