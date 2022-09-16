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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;
using Telegram.Bot.Framework.TelegramMessage;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Net.Example
{
    [BotName("FF", "GG")]
    public class Commands : TelegramController
    {
        private readonly IServiceProvider serviceProvider;
        public Commands(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Command(nameof(Test1), CommandInfo = "传递一个参数，询问并捕获一个参数")]
        public async Task Test1([Param("请输入要说的话：", true)] string Message)
        {
            await SendTextMessage("你好，你要说的话是：");
            await SendTextMessage(Message);
        }

        [Command(nameof(Test2), CommandInfo = "传递两个参数，询问并捕获两个参数")]
        public async Task Test2([Param("请输入第一句话：", true)] string FirstMessage, [Param("请输入第二句话：", true)] string TwoMessage)
        {
            await SendTextMessage($"你说的第一句是：{FirstMessage}");
            await SendTextMessage($"你说的第二句是：{TwoMessage}");
            await SendTextMessage($"合起来是：{FirstMessage}，{TwoMessage}。");
        }

        [Command(nameof(SayHello), CommandInfo = "输出一个简单的 Hello World")]
        public async Task SayHello()
        {
            await SendTextMessage("Hello World");
        }

        [Command(nameof(NowTime), CommandInfo = "输出现在的时间")]
        public async Task NowTime()
        {
            await SendTextMessage(DateTime.Now.ToString("现在的时间是 yyyy 年 MM 月 dd 日 HH 点 mm 分 ss 秒"));
        }

        [Command(nameof(CallBackTest), CommandInfo = "测试一个具有回调函数的方法")]
        public async Task CallBackTest()
        {
            await SendTextMessage("回调函数", new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("点击说你好",
                CreateCallBack((context, userscope)=>
                {
                    context.BotClient.SendTextMessageAsync(context.ChatID, "你好！！");
                }))
            });
        }

        [Command(nameof(Game1), CommandInfo = "通过回调函数实现的 剪子包袱锤 小游戏")]
        public async Task Game1()
        {
            await SendTextMessage("请选择你要出的项目", new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("剪子",
                CreateCallBack((context, userscope)=>
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
                })),
                InlineKeyboardButton.WithCallbackData("包袱",
                CreateCallBack((context, userscope) =>
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
                })),
                InlineKeyboardButton.WithCallbackData("锤",
                CreateCallBack((context, userscope) =>
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
                })),
            });
        }
    }
}
