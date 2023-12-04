//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Net;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Users
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TGChat : Update
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatId ChatId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider UserService => __UserServiceScope.ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        public IAuthenticate Authenticate { get; }

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceScope __UserServiceScope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScope"></param>
        /// <param name="chatId"></param>
        private TGChat(IServiceScope serviceScope, ChatId chatId)
        {
            __UserServiceScope = serviceScope;
            ChatId = chatId;

            Authenticate = UserService.GetRequiredService<IAuthenticate>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="telegramBot"></param>
        /// <param name="chatId"></param>
        /// <param name="BotService"></param>
        /// <returns></returns>
        public static TGChat GetChat(ITelegramBotClient telegramBot, ChatId chatId, IServiceProvider BotService)
        {
            TGChat chat = new(BotService.CreateScope(), chatId)
            {
                BotClient = telegramBot,
            };
            return chat;
        }
    }
}
