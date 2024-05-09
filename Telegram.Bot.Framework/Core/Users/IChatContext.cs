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

using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Users
{
    /// <summary>
    /// 聊天信息上下文
    /// </summary>
    public interface IChatContext
    {
        /// <summary>
        /// 请求的聊天ID
        /// </summary>
        public ChatId RequestChatID { get; }

        /// <summary>
        /// 请求用户的聊天ID
        /// </summary>
        public ChatId? RequestUserChatID { get; }

        /// <summary>
        /// 请求用户的信息
        /// </summary>
        public User? RequestUser { get; }

        /// <summary>
        /// 请求的聊天信息
        /// </summary>
        /// <returns>聊天信息</returns>
        public Task<Chat> RequestChat(CancellationToken cancellationToken);

        /// <summary>
        /// 请求的用户的聊天信息
        /// </summary>
        /// <returns>聊天信息</returns>
        public Task<Chat?> RequestUserChat(CancellationToken cancellationToken);
    }
}
