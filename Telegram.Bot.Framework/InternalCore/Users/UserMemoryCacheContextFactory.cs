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

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Framework.SimpleAuthentication;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalCore.Users
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IContextFactory))]
    internal class UserMemoryCacheContextFactory : IContextFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public UserMemoryCacheContextFactory(IMemoryCache memoryCache) =>
            this.memoryCache = memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botServiceProvider"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public TelegramUserContext? GetOrCreateUserContext(IServiceProvider botServiceProvider, Update update)
        {
            var filters = botServiceProvider.GetServices<IRequestFilter>();
            foreach (var filter in filters)
                if (!filter.Filter(update))
                    return null;

            var requestChatID = update.GetChatID();

            if (requestChatID == null)
                return null;

            TelegramUserContext? telegramUserContext = null;
            var longChatID = requestChatID.Identifier;
            if (longChatID != null)
            {
                var chatID = longChatID.Value;
                telegramUserContext = memoryCache.GetOrCreate(chatID, (cache) =>
                {
                    var telegramUserContext = new TelegramUserContext(botServiceProvider);
                    _ = cache.SetPriority(CacheItemPriority.NeverRemove);
                    _ = cache.SetValue(telegramUserContext);
                    return telegramUserContext;
                });
            }
            else if (!string.IsNullOrEmpty(requestChatID.Username))
            {
                var username = requestChatID.Username;
                telegramUserContext = memoryCache.GetOrCreate(username, (cache) =>
                {
                    var telegramUserContext = new TelegramUserContext(botServiceProvider);
                    _ = cache.SetPriority(CacheItemPriority.NeverRemove);
                    _ = cache.SetValue(telegramUserContext);
                    return telegramUserContext;
                });
            }

            telegramUserContext ??= new TelegramUserContext(botServiceProvider);
            telegramUserContext.CopyFrom(update);
            telegramUserContext.RequestChatID = requestChatID;
            return telegramUserContext;
        }
    }
}
