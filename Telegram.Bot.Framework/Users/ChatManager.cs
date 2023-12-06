﻿//  <Telegram.Bot.Framework>
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

using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Users
{
    /// <summary>
    /// 用于管理 <see cref="TGChat"/> 对象的管理服务，实现了 <see cref="IChatManager"/> 接口。<br></br>
    /// 拥有 <see cref="DependencyInjectionAttribute"/> 标签，可以自动注册服务
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IChatManager))]
    internal class ChatManager : IChatManager
    {
        /// <summary>
        /// 用于缓存 <see cref="TGChat"/> 对象
        /// </summary>
        private readonly Dictionary<ChatId, TGChat> __Chats = [];

        /// <summary>
        /// 创建或取得 <see cref="TGChat"/> 对象
        /// </summary>
        /// <remarks>
        /// 当缓存中没有指定用户的数据时，将创建一个新的 <see cref="TGChat"/> 对象，
        /// 如果缓存中已经有了 <see cref="TGChat"/> 对象，则从缓存中取出，并更新值
        /// </remarks>
        /// <param name="telegramBotClient">机器人接口</param>
        /// <param name="update">更新数据</param>
        /// <param name="BotServiceProvider">Bot级别的服务</param>
        /// <returns><see cref="TGChat"/> 对象</returns>
        public TGChat Create(ITelegramBotClient telegramBotClient, Update update, IServiceProvider BotServiceProvider)
        {
            var chatID = update.GetChatID();
            TGChat chat;
            if (chatID != null)
            {
                if (!__Chats.TryGetValue(chatID, out chat))
                {
                    chat = TGChat.GetChat(telegramBotClient, chatID, BotServiceProvider);
                    __Chats.Add(chatID, chat);
                }
            }
            else
                chat = TGChat.GetChat(telegramBotClient, chatID, BotServiceProvider);
            chat.CopyTo(update);
            return chat;
        }
    }
}
