//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core
{
    public sealed partial class TelegramContext : IChatContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatId RequestChatID { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public ChatId? RequestUserChatID => RequestUser?.Id == null ? null : new ChatId(RequestUser.Id);

        /// <summary>
        /// 
        /// </summary>
        public User? RequestUser => Extensions.GetUser(this);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Chat> RequestChat(CancellationToken cancellationToken) => 
            await BotClient.GetChatAsync(RequestChatID, cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Chat?> RequestUserChat(CancellationToken cancellationToken) => 
            RequestUserChatID == null ? null : await BotClient.GetChatAsync(RequestUserChatID, cancellationToken);
    }
}
