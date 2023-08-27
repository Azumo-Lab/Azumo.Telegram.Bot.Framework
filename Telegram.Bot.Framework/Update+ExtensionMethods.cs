using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Framework
{
    internal static class Update_ExtensionMethods
    {
        public static ChatId GetChatID(this Update update)
        {
            switch (update.Type)
            {
                case Types.Enums.UpdateType.Unknown:
                    break;
                case Types.Enums.UpdateType.Message:
                    return update.Message.Chat.Id;
                case Types.Enums.UpdateType.InlineQuery:
                    break;
                case Types.Enums.UpdateType.ChosenInlineResult:
                    break;
                case Types.Enums.UpdateType.CallbackQuery:
                    return update.CallbackQuery.Message.Chat.Id;
                case Types.Enums.UpdateType.EditedMessage:
                    return update.EditedMessage.Chat.Id;
                case Types.Enums.UpdateType.ChannelPost:
                    return update.ChannelPost.Chat.Id;
                case Types.Enums.UpdateType.EditedChannelPost:
                    return update.EditedChannelPost.Chat.Id;
                case Types.Enums.UpdateType.ShippingQuery:
                    break;
                case Types.Enums.UpdateType.PreCheckoutQuery:
                    break;
                case Types.Enums.UpdateType.Poll:
                    break;
                case Types.Enums.UpdateType.PollAnswer:
                    break;
                case Types.Enums.UpdateType.MyChatMember:
                    return update.MyChatMember.Chat.Id;
                case Types.Enums.UpdateType.ChatMember:
                    return update.ChatMember.Chat.Id;
                case Types.Enums.UpdateType.ChatJoinRequest:
                    return update.ChatJoinRequest.Chat.Id;
            }
            return null;
        }

        public static string GetCommand(this Update update)
        {
            if(update.Message == null || string.IsNullOrEmpty(update.Message.Text))
                return null;
            string command = update.Message.Text;
            if (command.StartsWith("/"))
                return command;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        internal static void SetUpdate(this Update souce, Update target)
        {
            souce.Poll = target.Poll;
            souce.ChatMember = target.ChatMember;
            souce.ChatJoinRequest = target.ChatJoinRequest;
            souce.CallbackQuery = target.CallbackQuery;
            souce.ChannelPost = target.ChannelPost;
            souce.ChosenInlineResult = target.ChosenInlineResult;
            souce.EditedChannelPost = target.EditedChannelPost;
            souce.EditedMessage = target.EditedMessage;
            souce.Id = target.Id;
            souce.InlineQuery = target.InlineQuery;
            souce.Message = target.Message;
            souce.MyChatMember = target.MyChatMember;
            souce.PollAnswer = target.PollAnswer;
            souce.PreCheckoutQuery = target.PreCheckoutQuery;
            souce.ShippingQuery = target.ShippingQuery;
        }
    }
}
