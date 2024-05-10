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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="telegramUserContext"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<Message> SendTextMessageAsync(this TelegramUserContext telegramUserContext, string message) =>
            await telegramUserContext.BotClient.SendTextMessageAsync(telegramUserContext.RequestChatID, message, parseMode: ParseMode.Html);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="telegramUserContext"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static async Task<Message> SendTextMessageAsync(this TelegramUserContext telegramUserContext, string message, List<InlineKeyboardButton> buttons) =>
            await telegramUserContext.BotClient.SendTextMessageAsync(telegramUserContext.RequestChatID, message, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
    }
}
