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
//

using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core;

/// <summary>
/// Telegram 请求上下文
/// </summary>
public interface IRequestContext : IDisposable
{
    /// <summary>
    /// 请求用户的ChatID
    /// </summary>
    /// <remarks>
    /// 向Bot发送请求的用户的ChatID
    /// </remarks>
    public ChatId RequestChatID { get; set; }

    /// <summary>
    /// 请求的用户
    /// </summary>
    /// <remarks>
    /// 向Bot发送请求的用户。<br></br>
    /// 如果是私聊，这个值和 <see cref="RequestChatID"/> 的用户是一样的。<br></br>
    /// 如果是群组或频道，这个值是向Bot发送消息的用户。<see cref="RequestChatID"/> 的值则是频道或群组
    /// </remarks>
    public User RequestUser { get; set; }
}
