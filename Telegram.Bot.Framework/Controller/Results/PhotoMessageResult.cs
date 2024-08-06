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
//
//  Author: 牛奶

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 发送带有图片的消息
    /// </summary>
    public class PhotoMessageResult : ActionResult<Message>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string PhotoName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="photoPath"></param>
        /// <param name="buttonResults"></param>
        public PhotoMessageResult(TelegramMessageBuilder message, string photoPath, ActionButtonResult[]? buttonResults = null)
        {
            Text = message;
            Files.Add(photoPath.OpenBufferedStream());
#if NET8_0_OR_GREATER
            ButtonResults.AddRange(buttonResults ?? []);
#else
            ButtonResults.AddRange(buttonResults ?? Array.Empty<ActionButtonResult>());
#endif
            PhotoName = Path.GetFileName(photoPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken) =>
            context.TelegramBotClient.SendChatActionAsync(context.ChatId!, ChatAction.UploadPhoto, cancellationToken: cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override IRequest<Message> ExecuteResultAsync(TelegramActionContext context)
        {
            var chatID = context.ChatId;
            return new SendPhotoRequest(chatID!, InputFile.FromStream(Files[0], PhotoName))
            {
                Caption = Text?.ToString(),
                ParseMode = Text?.ParseMode,
#if NET8_0_OR_GREATER
                ReplyMarkup = ButtonResults == null ? null : new InlineKeyboardMarkup(GetInlineKeyboardButtons(context, [.. ButtonResults])),
#else
                ReplyMarkup = ButtonResults == null ? null : new InlineKeyboardMarkup(GetInlineKeyboardButtons(context, ButtonResults.ToArray())),
#endif
                DisableNotification = Option?.DisableNotification,
                ReplyToMessageId = Option?.ReplyToMessageId,
            };
        }
    }
}
