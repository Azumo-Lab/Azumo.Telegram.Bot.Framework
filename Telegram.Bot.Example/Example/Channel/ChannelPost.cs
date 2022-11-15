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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Example.Example.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelPost : TelegramController
    {
        [Command("Channel_Post_Test", CommandInfo = "测试一下用机器人向测试频道发送消息")]
        public async Task SendChannel([Param("请输入你想发送的信息：")]string message)
        {
            IChannelManager channelManager = Context.UserScope.GetService<IChannelManager>();
            ChatId[] chatId = channelManager.GetActiveChannel(Context.TelegramUser);
            if (chatId == null || !chatId.Any())
            {
                await Context.SendTextMessage("你似乎没有没有在本Bot注册过频道");
                return;
            }
            await Context.BotClient.SendTextMessageAsync(chatId.First(), message);
            await Context.SendTextMessage("发送成功");
        }
    }
}
