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
using System.Threading;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    [DependencyInjection<IUserManager>(ServiceLifetime.Singleton)]
    internal class InternalUserManager : IUserManager
    {
        private int __UserCount;
        public int UserCount
        {
            get => __UserCount;
            set => __UserCount = value;
        }
        private readonly Dictionary<long, InternalChat> __ChatCache = new();
        public IChat CreateIChat(ITelegramBotClient botClient, Update update, IServiceScope BotService)
        {
            Chat chat = update.GetChat();
            if (chat == null)
                return null;

            if (__ChatCache.TryGetValue(chat.Id, out InternalChat iChat))
            {
                iChat.Request = new InternalRequest(update);
                iChat.ChatInfo.SendUser = update.GetSendUser();
                iChat.VisitTime = DateTime.Now;
            }
            else
            {
                iChat = new InternalChat(botClient, update, BotService);
                _ = __ChatCache.TryAdd(chat.Id, iChat);
                _ = Interlocked.Add(ref __UserCount, 1);
            }
            return iChat;
        }

        public void UpdateUserCount()
        {
            List<long> ___ = new();
            foreach (KeyValuePair<long, InternalChat> item in __ChatCache)
            {
                if (item.Value.HasRemove())
                    ___.Add(item.Key);
            }
            ___.ForEach(l =>
            {
                _ = __ChatCache.Remove(l);
                _ = Interlocked.Add(ref __UserCount, -1);
            });
        }
    }
}
