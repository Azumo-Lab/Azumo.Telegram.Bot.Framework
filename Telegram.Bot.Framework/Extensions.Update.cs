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

using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 从 <see cref="Update"/> 中获取 <see cref="ChatId"/> 对象
        /// </summary>
        /// <param name="update">传入的 <see cref="Update"/> 对象</param>
        /// <returns><see cref="ChatId"/> 对象</returns>
        public static ChatId GetChatID(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    return update.Message?.Chat?.Id!;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery!.Message!.Chat.Id;
                case UpdateType.EditedMessage:
                    return update.EditedMessage!.Chat.Id;
                case UpdateType.ChannelPost:
                    return update.ChannelPost!.Chat.Id;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost!.Chat.Id;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    return update.MyChatMember!.Chat.Id;
                case UpdateType.ChatMember:
                    return update.ChatMember!.Chat.Id;
                case UpdateType.ChatJoinRequest:
                    return update.ChatJoinRequest!.Chat.Id;
            }
            return null!;
        }

        /// <summary>
        /// 从 <see cref="Update"/> 中获取 <see cref="ChatId"/> 对象
        /// </summary>
        /// <param name="update">传入的 <see cref="Update"/> 对象</param>
        /// <returns><see cref="ChatId"/> 对象</returns>
        public static User GetUser(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    return update.Message?.From!;
                case UpdateType.InlineQuery:
                    return update.InlineQuery?.From!;
                case UpdateType.ChosenInlineResult:
                    return update.ChosenInlineResult?.From!;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery?.From!;
                case UpdateType.EditedMessage:
                    return update.EditedMessage?.From!;
                case UpdateType.ChannelPost:
                    return update.ChannelPost?.From!;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost?.From!;
                case UpdateType.ShippingQuery:
                    return update.ShippingQuery?.From!;
                case UpdateType.PreCheckoutQuery:
                    return update.PreCheckoutQuery?.From!;
                case UpdateType.Poll: // 发起投票
                    break;
                case UpdateType.PollAnswer:
                    return update.PollAnswer!.User;
                case UpdateType.MyChatMember:
                    return update.MyChatMember!.From;
                case UpdateType.ChatMember:
                    return update.ChatMember!.From;
                case UpdateType.ChatJoinRequest:
                    return update.ChatJoinRequest!.From;
            }
            return null!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static (string command, string[] paramsArray) GetCommandWithArgs(this Update update)
        {
            // 初始化变量
            var command = string.Empty;
            var paramsArray = new List<string>();

            var entitiesList = update.Message?.Entities;
            var entitiesValueList = update.Message?.EntityValues?.ToArray();

            var entity = entitiesList?.FirstOrDefault();
            if (entity == null)
                return (command, paramsArray.ToArray());

            if (entity.Type == MessageEntityType.BotCommand)
                command = entitiesValueList?.FirstOrDefault() ?? string.Empty;

            if (entitiesList != null && entitiesValueList != null)
                for (var i = 1; i < entitiesList.Length; i++)
                    paramsArray.Add(entitiesValueList[i]);

            return (command, paramsArray.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public static string GetCommand(this Update update)
        {
            var entitiesList = update.Message?.Entities;
            var entity = entitiesList?.FirstOrDefault();
            if (entity == null)
                return string.Empty;

            if (entity.Type != MessageEntityType.BotCommand)
                return string.Empty;

            var command = update.Message?.EntityValues?.FirstOrDefault();

            return command ?? string.Empty;
        }
    }
}
