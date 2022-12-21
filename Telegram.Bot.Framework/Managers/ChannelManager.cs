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
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Types;
using static Telegram.Bot.Framework.Abstract.IChannelManager;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 频道管理
    /// </summary>
    public class ChannelManager : IChannelManager
    {
        private readonly Dictionary<long, List<ChatId>> _channels = new();

        public event SaveChannelDelegate SaveChannelEvent;
        public event GetChannelDelegate GetChannelEvent;

        /// <summary>
        /// 获取用户对应的频道群组
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ChatId[] GetActiveChannel(TelegramUser user)
        {
            if (_channels.TryGetValue(user.Id, out List<ChatId> channels))
                return channels.ToArray();
            ChatId[] chatIds = GetChannelEvent?.Invoke(user.Id);
            channels = chatIds?.ToList();
            if (channels != null)
                _channels.TryAdd(user.Id, channels);
            return channels.ToArray();
        }

        /// <summary>
        /// 注册用户对应的频道群组
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channelId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RegisterChannel(TelegramUser user, params ChatId[] channelId)
        {
            if (_channels.ContainsKey(user.Id))
                _channels[user.Id].AddRange(channelId);
            else
                _channels.TryAdd(user.Id, channelId.ToList());
            SaveChannelEvent?.Invoke(user.Id, channelId);
        }
    }
}
