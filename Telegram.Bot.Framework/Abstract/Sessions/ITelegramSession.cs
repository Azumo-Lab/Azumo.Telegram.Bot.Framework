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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    /// <summary>
    /// 访问的请求会话
    /// </summary>
    public interface ITelegramSession
    {
        /// <summary>
        /// 会话是否已经结束
        /// </summary>
        public bool IsDispose { get; }

        /// <summary>
        /// 会话的用户
        /// </summary>
        public TelegramUser User { get; }

        /// <summary>
        /// Session的存储
        /// </summary>
        public ISession Session { get; }

        /// <summary>
        /// 请求信息
        /// </summary>
        public Update Update { get; }

        /// <summary>
        /// 用户范围内的服务
        /// </summary>
        public IServiceProvider UserService { get; }

        /// <summary>
        /// Bot客户端
        /// </summary>
        public ITelegramBotClient BotClient { get; }

        /// <summary>
        /// Bot启动关闭的控制
        /// </summary>
        public ITelegramBot TelegramBot { get; }
    }
}
