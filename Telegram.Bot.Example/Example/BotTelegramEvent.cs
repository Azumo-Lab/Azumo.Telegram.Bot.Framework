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

namespace Telegram.Bot.Example.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class BotTelegramEvent : IBotTelegramEvent
    {
        public async Task OnBeAdmin(TelegramContext context)
        {
            if (context.Update.MyChatMember.Chat.Type == Types.Enums.ChatType.Channel)
            {
                IChannelManager channelManager = context.UserScope.GetService<IChannelManager>();
                channelManager.RegisterChannel(context.TelegramUser, context.ChatID);
            }
            await Task.CompletedTask;
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
