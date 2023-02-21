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

namespace Telegram.Bot.Example.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelPostCommand : TelegramController
    {
        /// <summary>
        /// 转发频道，需要邀请Bot为频道的管理员
        /// </summary>
        /// <param name="PostMessage">要发送的消息</param>
        /// <returns></returns>
        [Command(nameof(ChannelPost), CommandInfo = "测试发送频道消息")]
        public async Task ChannelPost([Param("请输入要发送的消息，仅支持文本：")]string PostMessage)
        {
            IChannelManager channelManager = Context.UserScope.GetService<IChannelManager>();
            ChatId[] channelIDs = channelManager.GetActiveChannel(Context.TelegramUser);
            if (channelIDs == null)
            {
                await Context.SendTextMessage("你没有注册任何频道，请注册频道后再次重试");
                return;
            }
            ChatId channel = channelIDs.First();
            Chat channelChat = await Context.BotClient.GetChatAsync(channel);

            await Context.SendTextMessage($"向 {channelChat.Title} 发送消息： {PostMessage}");
            await Context.BotClient.SendTextMessageAsync(channel, PostMessage);
            await Context.SendTextMessage("发送成功");
        }
    }
}
