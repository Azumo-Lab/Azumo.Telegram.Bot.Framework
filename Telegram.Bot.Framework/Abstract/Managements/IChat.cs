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
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Managements
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChat : IDisposable
    {
        /// <summary>
        /// 这个Chat的ID
        /// </summary>
        public long ChatID { get; internal set; }

        /// <summary>
        /// Chat的类型
        /// </summary>
        public ChatType ChatType { get; internal set; }

        /// <summary>
        /// 是否绝交
        /// </summary>
        public bool IsBan { get; set; }

        /// <summary>
        /// 机器人
        /// </summary>
        public ITelegramBotClient TelegramBotClient { get; internal set; }

        /// <summary>
        /// Chat范围内的服务
        /// </summary>
        public IServiceScope ChatServiceScope { get; internal set; }

        /// <summary>
        /// 用于存储数据的Session
        /// </summary>
        public ISession Session { get; internal set; }

        public ITelegramRequest TelegramRequest { get; internal set; }
    }
}
