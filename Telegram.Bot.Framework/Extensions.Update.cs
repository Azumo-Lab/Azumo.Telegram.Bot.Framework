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
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.SimpleAuthentication;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="update"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="updateFilters"></param>
    /// <returns></returns>
    internal static IRequestContext? GetRequestContext(this Update update, IServiceProvider serviceProvider, IEnumerable<IRequestFilter> updateFilters)
    {
        foreach (var item in updateFilters)
            if (!item.Filter(update))
                return null;

        var requestContext = serviceProvider.GetRequiredService<IRequestContext>();

        requestContext.RequestUser = update.GetUser();
        requestContext.RequestChatID = update.GetChatID();

        return requestContext;
    }

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
                return update.CallbackQuery!.Message!.Chat.Id;
            case Types.Enums.UpdateType.EditedMessage:
                return update.EditedMessage!.Chat.Id;
            case Types.Enums.UpdateType.ChannelPost:
                return update.ChannelPost!.Chat.Id;
            case Types.Enums.UpdateType.EditedChannelPost:
                return update.EditedChannelPost!.Chat.Id;
            case Types.Enums.UpdateType.ShippingQuery:
                break;
            case Types.Enums.UpdateType.PreCheckoutQuery:
                break;
            case Types.Enums.UpdateType.Poll:
                break;
            case Types.Enums.UpdateType.PollAnswer:
                break;
            case Types.Enums.UpdateType.MyChatMember:
                return update.MyChatMember!.Chat.Id;
            case Types.Enums.UpdateType.ChatMember:
                return update.ChatMember!.Chat.Id;
            case Types.Enums.UpdateType.ChatJoinRequest:
                return update.ChatJoinRequest!.Chat.Id;
        }
        return null!;
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
            case Types.Enums.UpdateType.Poll: // 发起投票
                break;
            case Types.Enums.UpdateType.PollAnswer:
                return update.PollAnswer!.User;
            case Types.Enums.UpdateType.MyChatMember:
                return update.MyChatMember!.From;
            case Types.Enums.UpdateType.ChatMember:
                return update.ChatMember!.From;
            case Types.Enums.UpdateType.ChatJoinRequest:
                return update.ChatJoinRequest!.From;
        }
        return null;
    }
}
