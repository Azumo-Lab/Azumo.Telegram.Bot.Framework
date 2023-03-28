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

using Telegram.Bot.Framework.Abstract.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Channels
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelManager
    {
        public delegate void SaveChannelDelegate(long ID, ChatId[] chatID);
        public delegate ChatId[] GetChannelDelegate(long ID);

        /// <summary>
        /// 用户注册频道时候的事件
        /// </summary>
        public event SaveChannelDelegate SaveChannelEvent;

        /// <summary>
        /// 获取频道时候的事件
        /// </summary>
        public event GetChannelDelegate GetChannelEvent;

        /// <summary>
        /// 获取一个频道
        /// </summary>
        ChatId[] GetActiveChannel(TelegramUser user);

        /// <summary>
        /// 用户注册一个或几个频道
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="channelId">频道的ChatID</param>
        void RegisterChannel(TelegramUser user, params ChatId[] channelId);
    }
}
