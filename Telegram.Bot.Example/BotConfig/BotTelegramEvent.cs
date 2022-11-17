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
using Telegram.Bot.Example.DataBase;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Example.BotConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class BotTelegramEvent : IBotTelegramEvent
    {
        private readonly TelegramBotContext telegramBotContext;
        public BotTelegramEvent(TelegramBotContext telegramBotContext)
        {
            this.telegramBotContext = telegramBotContext;
        }

        public async Task OnBeAdmin(TelegramContext context)
        {
            if (context.Update.MyChatMember.Chat.Type == Types.Enums.ChatType.Channel)
            {
                IChannelManager channelManager = context.UserScope.GetService<IChannelManager>();
                channelManager.SaveChannelEvent += ChannelManager_SaveChannelEvent;
                channelManager.GetChannelEvent += ChannelManager_GetChannelEvent;
                channelManager.RegisterChannel(context.TelegramUser, context.ChatID);
            }
            await Task.CompletedTask;
        }

        private ChatId[] ChannelManager_GetChannelEvent(long ID)
        {
            List<ChatId> list = telegramBotContext.UserChannels.Where(x => x.UserID == ID).ToList().Select(x => new ChatId(x.ChannelChatID)).ToList();
            return list.ToArray();
        }

        private void ChannelManager_SaveChannelEvent(long ID, Types.ChatId[] chatID)
        {
            foreach (Types.ChatId item in chatID)
            {
                telegramBotContext.UserChannels.Add(new DataBase.Datas.UserChannels
                {
                    ChannelChatID = (long)item.Identifier,
                    UserID = ID
                });
            }
            telegramBotContext.SaveChanges();
        }

        public async Task OnCreator(TelegramContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnInvited(TelegramContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnKicked(TelegramContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnLeft(TelegramContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnRestricted(TelegramContext context)
        {
            await Task.CompletedTask;
        }
    }
}
