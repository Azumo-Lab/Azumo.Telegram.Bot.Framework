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
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller
{
    /// <summary>
    /// Telegram 一个请求
    /// </summary>
    public sealed class TelegramRequest : Update
    {
        /// <summary>
        /// 发送的消息内容
        /// </summary>
        public string? MessageText { get; }

        /// <summary>
        /// 机器人指令
        /// </summary>
        public string? BotCommand
        {
            get
            {
                if (MessageEntities.Count == 0)
                    return null;
                var messageInfo = MessageEntities[0];
                return messageInfo != null && messageInfo.Type == MessageEntityType.BotCommand ? messageInfo.Value : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<MessageEntityInfo> Params => MessageEntities.Skip(1).ToList();

        /// <summary>
        /// 
        /// </summary>
        public ITelegramBotClient TelegramBotClient { get; }

        /// <summary>
        /// 
        /// </summary>
        public UserPermissions UserPermissions { get; } = new UserPermissions();

        /// <summary>
        /// 
        /// </summary>
        private readonly List<MessageEntityInfo> _messageEntities =
#if NET8_0_OR_GREATER
            [];
#else
            new List<MessageEntityInfo>();
#endif

        /// <summary>
        /// 
        /// </summary>
        public ChatId? ChatId { get; }

        /// <summary>
        /// 
        /// </summary>
        public User? RequestUser { get; }

        /// <summary>
        /// 
        /// </summary>
        public Chat? Chat { get; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<MessageEntityInfo> MessageEntities => _messageEntities;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="telegramBotClient"></param>
        internal TelegramRequest(Update update, ITelegramBotClient telegramBotClient)
        {
            Message = update.Message;
            Id = update.Id;
            EditedMessage = update.EditedMessage;
            ChannelPost = update.ChannelPost;
            CallbackQuery = update.CallbackQuery;
            ChatJoinRequest = update.ChatJoinRequest;
            ChatMember = update.ChatMember;
            ChosenInlineResult = update.ChosenInlineResult;
            MyChatMember = update.MyChatMember;
            PollAnswer = update.PollAnswer;
            Poll = update.Poll;
            PreCheckoutQuery = update.PreCheckoutQuery;
            ShippingQuery = update.ShippingQuery;
            InlineQuery = update.InlineQuery;
            EditedChannelPost = update.EditedChannelPost;

            TelegramBotClient = telegramBotClient;

            switch (Type)
            {
                case UpdateType.Unknown:
                    return;
                case UpdateType.Message:
                    List<MessageEntity> messageEntity;
                    List<string> messageEntityValues;

                    MessageText = Message?.Text;

#if NET8_0_OR_GREATER
                    messageEntity = new List<MessageEntity>(Message?.Entities ?? []);
                    messageEntityValues = new List<string>(Message?.EntityValues ?? []);
#else
                    messageEntity = new List<MessageEntity>(Message?.Entities ?? new MessageEntity[0]);
                    messageEntityValues = new List<string>(Message?.EntityValues ?? new string[0]);
#endif
                    var size = messageEntity.Count;

                    if (size != messageEntityValues.Count)
                        return;

                    for (var i = 0; i < size; i++)
                    {
                        var item = messageEntity[i];
                        if (item is null)
                            continue;
                        _messageEntities.Add(new MessageEntityInfo
                        {
                            Value = messageEntityValues[i],
                            CustomEmojiId = item.CustomEmojiId,
                            Language = item.Language,
                            Length = item.Length,
                            Offset = item.Offset,
                            Type = item.Type,
                            Url = item.Url,
                            User = item.User
                        });
                    }
                    return;
                case UpdateType.InlineQuery:

                    return;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:

                    break;
                default:
                    break;
            }
        }
    }
}
