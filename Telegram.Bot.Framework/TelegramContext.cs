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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 机器人的各类信息
    /// </summary>
    public sealed class TelegramContext
    {
        /// <summary>
        /// 机器人接口
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; }

        /// <summary>
        /// 接收的信息
        /// </summary>
        public Update Update { get; internal set; }

        /// <summary>
        /// Token
        /// </summary>
        public CancellationToken CancellationToken { get; internal set; }

        /// <summary>
        /// 只有一次的Scope
        /// </summary>
        public IServiceProvider OneTimeScope { get; internal set; }

        /// <summary>
        /// 用户整个生命周期的服务
        /// </summary>
        public IServiceProvider UserScope { get; internal set; }

        /// <summary>
        /// 获取ChatID
        /// </summary>
        public long? ChatID => GetChatID(Update);

        /// <summary>
        /// 获取权限
        /// </summary>
        public AuthenticationRole AuthenticationRole => GetAuthenticationRole();

        /// <summary>
        /// 当前用户
        /// </summary>
        public TelegramUser TelegramUser { get; set; }

        
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private AuthenticationRole GetAuthenticationRole()
        {
            IAuthManager authManager = UserScope.GetService<IAuthManager>();
            return authManager.GetAuthenticationRole();
        }

        internal TelegramContext() {}

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <returns>返回Command</returns>
        public string GetCommand()
        {
            MessageEntity[] entities = Update.Message?.Entities;
            if (!entities.IsEmpty()
                && entities.FirstOrDefault().Type == MessageEntityType.BotCommand)
            {
                return Update.Message.EntityValues.FirstOrDefault()?.ToLower();
            }
            return null;
        }

        /// <summary>
        /// 获取ChatID（聊天窗口的ID）
        /// </summary>
        /// <param name="Update"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static long? GetChatID(Update Update) => Update.Type switch
        {
            UpdateType.CallbackQuery => Update.CallbackQuery.Message.Chat.Id,
            UpdateType.MyChatMember => Update.MyChatMember.Chat.Id,
            UpdateType.Message => Update.Message.Chat.Id,
            UpdateType.InlineQuery => null,
            UpdateType.ChosenInlineResult => null,
            UpdateType.EditedMessage => Update.EditedMessage.Chat.Id,
            UpdateType.ChannelPost => Update.ChannelPost.Chat.Id,
            UpdateType.EditedChannelPost => Update.EditedChannelPost.Chat.Id,
            UpdateType.ShippingQuery => null,
            UpdateType.PreCheckoutQuery => null,
            UpdateType.Poll => null,
            UpdateType.PollAnswer => null,
            UpdateType.ChatMember => Update.ChatMember.Chat.Id,
            UpdateType.ChatJoinRequest => Update.ChatJoinRequest.Chat.Id,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static TelegramUser GetTelegramUser(Update update)
        {
            return update.Type switch
            {
                UpdateType.Message => new TelegramUser(update.Message.From, GetChatID(update)),
                UpdateType.InlineQuery => new TelegramUser(update.InlineQuery.From),
                UpdateType.ChosenInlineResult => new TelegramUser(update.ChosenInlineResult.From),
                UpdateType.CallbackQuery => new TelegramUser(update.CallbackQuery.Message.From, GetChatID(update)),
                UpdateType.EditedMessage => new TelegramUser(update.EditedMessage.From, GetChatID(update)),
                UpdateType.ChannelPost => new TelegramUser(update.ChannelPost.From, GetChatID(update)),
                UpdateType.EditedChannelPost => new TelegramUser(update.EditedChannelPost.From, GetChatID(update)),
                UpdateType.ShippingQuery => new TelegramUser(update.ShippingQuery.From),
                UpdateType.PreCheckoutQuery => new TelegramUser(update.PreCheckoutQuery.From),
                UpdateType.Poll => null,
                UpdateType.PollAnswer => new TelegramUser(update.PollAnswer.User),
                UpdateType.MyChatMember => new TelegramUser(update.MyChatMember.From, GetChatID(update)),
                UpdateType.ChatMember => new TelegramUser(update.ChatMember.From, GetChatID(update)),
                UpdateType.ChatJoinRequest => new TelegramUser(update.ChatJoinRequest.From, GetChatID(update)),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
