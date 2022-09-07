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

        [Command("Test")]
        public async Task Test([Param("请输入要说的话：", true)] string Message)
        {
            await SendTextMessage("你好，你要说的话是：");
            await SendTextMessage(Message);
        }

        [Command("Test2")]
        public async Task Test([Param("请输入第一句话：", true)] string FirstMessage, [Param("请输入第二句话：", true)] string TwoMessage)
        {
            await SendTextMessage($"你说的第一句是：{FirstMessage}");
            await SendTextMessage($"你说的第二句是：{TwoMessage}");
            await SendTextMessage($"合起来是：{FirstMessage}，{TwoMessage}。");
        }

        [Command("SayHello")]
        public async Task SayHello()
        {
            await SendTextMessage("Hello World");
        }
    }
}
