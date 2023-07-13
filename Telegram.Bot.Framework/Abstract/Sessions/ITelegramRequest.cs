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
using Telegram.Bot.Framework.Abstract.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    /// <summary>
    /// Telegram 请求接口
    /// </summary>
    public interface ITelegramRequest
    {
        /// <summary>
        /// 请求信息
        /// </summary>
        public Update Update { get; internal set; }

        /// <summary>
        /// 整个Bot范围内的服务
        /// </summary>
        public IServiceScope BotScopeService { get; internal set; }

        /// <summary>
        /// 请求的用户
        /// </summary>
        public TelegramUser RequestUser { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Message GetMessage();
    }
}
