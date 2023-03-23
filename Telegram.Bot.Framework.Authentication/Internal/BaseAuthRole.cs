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
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Authentication.Interface;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Authentication.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class BaseAuthRole : IAuthenticationRole
    {
        public abstract BotCommandScopeType Type { get; }

        public abstract Task ChangeRole(TelegramSession session);

        public BotCommandScope GetBotCommandScope(TelegramSession session)
        {
            ChatId? chatID;
            switch (Type)
            {
                case BotCommandScopeType.Default:
                    return BotCommandScope.Default();
                case BotCommandScopeType.AllPrivateChats:
                    return BotCommandScope.AllPrivateChats();
                case BotCommandScopeType.AllGroupChats:
                    return BotCommandScope.AllGroupChats();
                case BotCommandScopeType.AllChatAdministrators:
                    return BotCommandScope.AllChatAdministrators();
                case BotCommandScopeType.Chat:
                    chatID = session.User.ChatID;
                    if (chatID!.IsNull())
                        return BotCommandScope.Default();
                    return BotCommandScope.Chat(chatID!);
                case BotCommandScopeType.ChatAdministrators:
                    chatID = session.User.ChatID;
                    if (chatID!.IsNull())
                        return BotCommandScope.Default();
                    return BotCommandScope.ChatAdministrators(chatID!);
                case BotCommandScopeType.ChatMember:
                    chatID = session.User.ChatID;
                    if (chatID!.IsNull())
                        return BotCommandScope.Default();
                    return BotCommandScope.ChatMember(chatID!, 10);
                default:
                    break;
            }
            return default!;
        }
    }
}
