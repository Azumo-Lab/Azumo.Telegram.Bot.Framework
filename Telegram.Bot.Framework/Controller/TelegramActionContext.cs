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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Telegram.Bot.Framework.Storage;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// Telegram 指令 上下文
    /// </summary>
    public sealed class TelegramActionContext
    {
        /// <summary>
        /// Telegram请求
        /// </summary>
        public TelegramRequest TelegramRequest { get; }

        /// <summary>
        /// Telegram上下文
        /// </summary>
        public TelegramContext TelegramContext { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider => TelegramContext.ScopeServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        internal TelegramActionContext(TelegramContext telegramContext, TelegramRequest telegramRequest, CancellationToken cancellationToken)
        {
            TelegramContext = telegramContext;
            TelegramRequest = telegramRequest;
            CancellationToken = cancellationToken;

            ChatId = telegramRequest.ChatId;
            TelegramBotClient = telegramRequest.TelegramBotClient;
            CommandScopeService = ServiceProvider.GetRequiredService<ICommandScopeService>();
        }

        /// <summary>
        /// 
        /// </summary>
        public ChatId? ChatId { get; }

        /// <summary>
        /// 
        /// </summary>
        internal ICommandScopeService CommandScopeService { get; }

        /// <summary>
        /// 
        /// </summary>
        public ISession Session => TelegramContext.Session;

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient TelegramBotClient { get; }

        /// <summary>
        /// 
        /// </summary>
        public CancellationToken CancellationToken { get; }
    }
}
