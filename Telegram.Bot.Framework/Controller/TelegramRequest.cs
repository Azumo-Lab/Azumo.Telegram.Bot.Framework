//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading.Tasks;
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
        /// 
        /// </summary>
        public bool NeedTelegramContext { get; private set; }

        /// <summary>
        /// 机器人指令
        /// </summary>
        /// <remarks>
        /// 机器人指令可以附带参数，例如：<br></br>
        /// <code>
        /// /start abc 123
        /// </code>
        /// 可以获取到的指令为：
        /// <code>
        /// /start
        /// </code>
        /// </remarks>
        public string? BotCommand
        {
            get
            {
                switch (Type)
                {
                    case UpdateType.Unknown:
                        break;
                    case UpdateType.Message:
                        if (MessageEntities.Count == 0)
                            return null;
                        var messageInfo = MessageEntities[0];
                        return messageInfo != null && messageInfo.Type == MessageEntityType.BotCommand ? messageInfo.Value : null;
                    case UpdateType.InlineQuery:
                        break;
                    case UpdateType.ChosenInlineResult:
                        break;
                    case UpdateType.CallbackQuery:
                        return $"/{CallbackQuery?.Data}";
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
                return null;
            }
        }

        /// <summary>
        /// 获取机器人指令的参数
        /// </summary>
        /// <remarks>
        /// 机器人指令可以附带参数，例如：<br></br>
        /// <code>
        /// /start abc 123
        /// </code>
        /// 可以获取到的参数为：
        /// <code>
        /// abc 123
        /// </code>
        /// </remarks>
        public IReadOnlyList<MessageEntityInfo> Params
        {
            get
            {
                if (MessageEntities.Count == 0)
                    return Empty;
                var messageInfo = MessageEntities[0];
                return messageInfo != null && messageInfo.Type == MessageEntityType.BotCommand ? MessageEntities.Skip(1).ToList() : Empty;
            }
        }

        /// <summary>
        /// 用户的聊天信息，或者群组的聊天信息
        /// </summary>
        public Chat? Chat { get; }

        /// <summary>
        /// Chat ID
        /// </summary>
        /// <remarks>
        /// Chat ID，这个ID在 <see cref="ChatType.Private"/> 时候，为发送用户的User ID <br></br>
        /// 在 <see cref="ChatType.Channel"/> <see cref="ChatType.Group"/> <see cref="ChatType.Supergroup"/> 时，则为群组的ID
        /// </remarks>
        public ChatId? ChatId { get; }

        /// <summary>
        /// 发送用户的ID
        /// </summary>
        public User? RequestUser { get; }

        /// <summary>
        /// 请求用户的Chat ID
        /// </summary>
        public ChatId? RequestUserChatID => 
            RequestUser == null ? null : new ChatId(RequestUser.Id);

        /// <summary>
        /// 获取请求用户的聊天信息
        /// </summary>
        public Task<Chat> RequestUserChat
        {
            get
            {
                if (RequestUserChatID == null || _RequestUserChat != null)
                    return _RequestUserChat;
                var result = TelegramBotClient.GetChatAsync(RequestUserChatID);
                _RequestUserChat = result;
                return _RequestUserChat;
            }
        }

        private Task<Chat> _RequestUserChat = Task.FromResult<Chat>(null!);

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
        public IReadOnlyList<MessageEntityInfo> MessageEntities => _messageEntities;

        /// <summary>
        /// 
        /// </summary>
        private readonly static IReadOnlyList<MessageEntityInfo> Empty = new List<MessageEntityInfo>();

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

            string? messageText = null;
            Chat? chat = null;
            ChatId? chatId = null;
            User? requestUser = null;

            switch (Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    ReadMessage(Message!, out messageText, out chat, out chatId, out requestUser, ref _messageEntities);
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    var message = CallbackQuery?.Message;
                    if (message != null)
                    {
                        ReadMessage(CallbackQuery?.Message!, out messageText, out chat, out chatId, out requestUser, ref _messageEntities);
                    }
                    break;
                case UpdateType.EditedMessage:
                    ReadMessage(EditedMessage!, out messageText, out chat, out chatId, out requestUser, ref _messageEntities);
                    break;
                case UpdateType.ChannelPost:
                    ReadMessage(ChannelPost!, out messageText, out chat, out chatId, out requestUser, ref _messageEntities);
                    break;
                case UpdateType.EditedChannelPost:
                    ReadMessage(EditedChannelPost!, out messageText, out chat, out chatId, out requestUser, ref _messageEntities);
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

            MessageText = messageText;
            Chat = chat;
            ChatId = chatId;
            RequestUser = requestUser;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageText"></param>
        /// <param name="chat"></param>
        /// <param name="chatId"></param>
        /// <param name="requestUser"></param>
        /// <param name="messageEntityInfo"></param>
        private static void ReadMessage(Message message, out string? messageText, out Chat? chat, out ChatId? chatId, out User? requestUser, ref List<MessageEntityInfo> messageEntityInfo)
        {
            List<MessageEntity> messageEntity;
            List<string> messageEntityValues;

            messageText = message.Text;
            chat = message.Chat;
            chatId = chat?.Id;
            requestUser = message.From;

#if NET8_0_OR_GREATER
            messageEntity = new List<MessageEntity>(message.Entities ?? []);
            messageEntityValues = new List<string>(message.EntityValues ?? []);
#else
            messageEntity = new List<MessageEntity>(message.Entities ?? new MessageEntity[0]);
            messageEntityValues = new List<string>(message.EntityValues ?? new string[0]);
#endif

            var size = messageEntity.Count;

            if (size != messageEntityValues.Count)
                return;

            for (var i = 0; i < size; i++)
            {
                var item = messageEntity[i];
                if (item is null)
                    continue;
                messageEntityInfo.Add(new MessageEntityInfo(item, messageEntityValues[i]));
            }
        }
    }
}
