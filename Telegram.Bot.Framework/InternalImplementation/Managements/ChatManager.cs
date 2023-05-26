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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Managements;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Managements
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class ChatManager : IChatManager
    {
        private readonly ConcurrentDictionary<long, IChat> Chats = new();

        public IChat GetChat(ITelegramRequest telegramRequest)
        {
            Chat chat = TelegramRequestManager.GetChat(telegramRequest.Update);
            if (chat != null && Chats.TryGetValue(chat.Id, out IChat iChat))
                return iChat;
            else
            {
                switch (chat.Type)
                {
                    case Types.Enums.ChatType.Private:
                        iChat = telegramRequest.BotScopeService.ServiceProvider.GetService<IPrivateChat>();
                        break;
                    case Types.Enums.ChatType.Group:
                        iChat = telegramRequest.BotScopeService.ServiceProvider.GetService<IGroup>();
                        break;
                    case Types.Enums.ChatType.Channel:
                        iChat = telegramRequest.BotScopeService.ServiceProvider.GetService<IChannel>();
                        break;
                    case Types.Enums.ChatType.Supergroup:
                        iChat = telegramRequest.BotScopeService.ServiceProvider.GetService<IGroup>();
                        break;
                    case Types.Enums.ChatType.Sender:
                        return default;
                    default:
                        return default;
                }
                iChat.Session = telegramRequest.BotScopeService.ServiceProvider.GetService<ISession>();
                iChat.ChatServiceScope = telegramRequest.BotScopeService.ServiceProvider.CreateScope();
                iChat.ChatType = chat.Type;
                iChat.ChatID = chat.Id;
                iChat.TelegramRequest = telegramRequest;
                iChat.TelegramBotClient = telegramRequest.BotScopeService.ServiceProvider.GetService<ITelegramBotClient>();

                if (Chats.TryAdd(chat.Id, iChat) && Chats.TryGetValue(chat.Id, out iChat))
                    return iChat;
            }
            return default;
        }
    }
}
