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

namespace Telegram.Bot.Framework.InternalFramework.Managers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelManager : IChannelManager
    {
        private readonly Dictionary<long, ChatId[]> _channels = new Dictionary<long, ChatId[]>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ChatId[] GetActiveChannel(TelegramUser user)
        {
            _channels.TryGetValue(user.Id, out ChatId[] channels);
            return channels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channelId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void RegisterChannel(TelegramUser user, params ChatId[] channelId)
        {
            _channels.TryAdd(user.Id, channelId);
        }
    }
}
