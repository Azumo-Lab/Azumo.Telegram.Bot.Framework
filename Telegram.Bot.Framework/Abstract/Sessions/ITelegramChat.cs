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
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstract.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITelegramChat : IDisposable
    {
        #region 聊天的基本信息

        /// <summary>
        /// 这个Chat的信息
        /// </summary>
        public Chat Chat { get; internal set; }

        #endregion

        #region 基础服务

        /// <summary>
        /// 请求
        /// </summary>
        public ITelegramRequest Request { get; internal set; }

        /// <summary>
        /// Chat范围的服务提供
        /// </summary>
        public IServiceProvider ChatService { get; }

        /// <summary>
        /// Bot的客户端
        /// </summary>
        public ITelegramBotClient BotClient { get; }

        #endregion

        #region 各类服务

        /// <summary>
        /// 指令相关的服务
        /// </summary>
        public ICommandService CommandService { get; }

        /// <summary>
        /// 
        /// </summary>
        public ICallBackService CallBackManager { get; }

        /// <summary>
        /// Session存储
        /// </summary>
        public ISession Session { get; }

        #endregion
    }
}
