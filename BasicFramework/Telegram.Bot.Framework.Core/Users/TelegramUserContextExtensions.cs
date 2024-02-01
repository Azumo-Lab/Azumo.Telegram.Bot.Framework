using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Users;
public static class TelegramUserContextExtensions
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
            case Types.Enums.UpdateType.Unknown:
                break;
            case Types.Enums.UpdateType.Message:
                return update.Message?.Chat?.Id!;
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

    /// <summary>
    /// 从 <see cref="Update"/> 中获取 <see cref="ChatId"/> 对象
    /// </summary>
    /// <param name="update">传入的 <see cref="Update"/> 对象</param>
    /// <returns><see cref="ChatId"/> 对象</returns>
    public static User? GetUser(this Update update)
    {
        switch (update.Type)
        {
            case Types.Enums.UpdateType.Unknown:
                break;
            case Types.Enums.UpdateType.Message:
                return update.Message?.From;
            case Types.Enums.UpdateType.InlineQuery:
                return update.InlineQuery?.From;
            case Types.Enums.UpdateType.ChosenInlineResult:
                return update.ChosenInlineResult?.From;
            case Types.Enums.UpdateType.CallbackQuery:
                return update.CallbackQuery?.From;
            case Types.Enums.UpdateType.EditedMessage:
                return update.EditedMessage?.From;
            case Types.Enums.UpdateType.ChannelPost:
                return update.ChannelPost?.From;
            case Types.Enums.UpdateType.EditedChannelPost:
                return update.EditedChannelPost?.From;
            case Types.Enums.UpdateType.ShippingQuery:
                return update.ShippingQuery?.From;
            case Types.Enums.UpdateType.PreCheckoutQuery:
                return update.PreCheckoutQuery?.From;
            case Types.Enums.UpdateType.Poll:
                break;
            case Types.Enums.UpdateType.PollAnswer:
                return update.PollAnswer.User;
            case Types.Enums.UpdateType.MyChatMember:
                return update.MyChatMember.From;
            case Types.Enums.UpdateType.ChatMember:
                return update.ChatMember.From;
            case Types.Enums.UpdateType.ChatJoinRequest:
                return update.ChatJoinRequest.From;
        }
        return null;
    }
}
