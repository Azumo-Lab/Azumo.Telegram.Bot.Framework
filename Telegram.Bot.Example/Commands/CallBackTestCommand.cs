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
using Telegram.Bot.Framework.Components;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Example.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CallBackTestCommand : TelegramController
    {
        [Command(nameof(CallBackTest), CommandInfo = "测试一个具有回调函数的方法")]
        public async Task CallBackTest()
        {
            await Context.SendTextMessage("回调函数", new List<InlineButtons>
            {
                InlineButtons.WithCallback("点击说你好",
                (context)=>
                {
                    context.BotClient.SendTextMessageAsync(context.ChatID, "你好！！");
                })
            });
            await Context.SendTextMessage("这是测试回调函数的用例，点击“点击说你好”按钮后，会触发发送“你好！！”的方法");
        }

        [Command(nameof(Game1), CommandInfo = "通过回调函数实现的简易 剪子包袱锤 小游戏")]
        public async Task Game1()
        {
            await Context.SendTextMessage("这个方法使用回调函数( /CallBackTest )实现，点击按钮之后触发判断。");
            await Context.SendTextMessage("请选择你要出的项目", new List<InlineButtons>
            {
                InlineButtons.WithCallback("剪子",
                (context)=>
                {
                    string[] item = { "剪子", "包袱", "锤" };
                    int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, 3);
                    if (index == 0)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 是平局");
                        return;
                    }
                    if (index == 1)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 啊啊啊，我输了");
                        return;
                    }
                    if (index == 2)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 嘿嘿嘿，你输了");
                        return;
                    }
                }),
                InlineButtons.WithCallback("包袱",
                (context) =>
                {
                    string[] item = { "剪子", "包袱", "锤" };
                    int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, 3);
                    if (index == 0)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 嘿嘿嘿，你输了");
                        return;
                    }
                    if (index == 1)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 是平局");
                        return;
                    }
                    if (index == 2)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 啊啊啊，我输了");
                        return;
                    }
                }),
                InlineButtons.WithCallback("锤",
                (context) =>
                {
                    string[] item = { "剪子", "包袱", "锤" };
                    int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, 3);
                    if (index == 0)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 啊啊啊，我输了");
                        return;
                    }
                    if (index == 1)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 嘿嘿嘿，你输了");
                        return;
                    }
                    if (index == 2)
                    {
                        context.BotClient.SendTextMessageAsync(context.ChatID, $"我出{item[index]}, 是平局");
                        return;
                    }
                }),
            });
        }
    }
}
