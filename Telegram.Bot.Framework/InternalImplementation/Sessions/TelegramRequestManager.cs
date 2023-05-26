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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class TelegramRequestManager
    {
        /// <summary>
        /// 唯一的实例
        /// </summary>
        public static TelegramRequestManager Instance { get; } = new TelegramRequestManager();

        private TelegramRequestManager()
        {
            
        }

        public static ITelegramRequest GetTelegramRequest(IServiceScope serviceScope, Update update)
        {
            ITelegramRequest request = serviceScope.ServiceProvider.GetService<ITelegramRequest>();
            request.Update = update;
            request.BotScopeService = serviceScope;
            return request;
        }

        public ITelegramSession GetTelegramSession()
        {
            return default;
        }

        public static Chat GetChat(Update update)
        {
            return update.Type switch
            {
                Types.Enums.UpdateType.Message => update?.Message?.Chat,
                Types.Enums.UpdateType.CallbackQuery => update?.CallbackQuery?.Message?.Chat,
                Types.Enums.UpdateType.EditedMessage => update?.EditedMessage?.Chat,
                Types.Enums.UpdateType.ChannelPost => update?.ChannelPost?.Chat,
                Types.Enums.UpdateType.EditedChannelPost => update?.EditedChannelPost?.Chat,
                Types.Enums.UpdateType.MyChatMember => update?.MyChatMember?.Chat,
                Types.Enums.UpdateType.ChatMember => update?.ChatMember?.Chat,
                Types.Enums.UpdateType.ChatJoinRequest => update?.ChatJoinRequest?.Chat,
                Types.Enums.UpdateType.Poll or Types.Enums.UpdateType.Unknown or Types.Enums.UpdateType.PollAnswer or Types.Enums.UpdateType.InlineQuery or Types.Enums.UpdateType.ShippingQuery or Types.Enums.UpdateType.PreCheckoutQuery or Types.Enums.UpdateType.ChosenInlineResult => default!,
                _ => default!,
            };
        }
    }
}
