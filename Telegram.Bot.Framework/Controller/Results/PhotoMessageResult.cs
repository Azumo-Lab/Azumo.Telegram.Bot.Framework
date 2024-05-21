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
        /// 要发送的图片路径
        /// </summary>
        private readonly string[] PhotoPaths;

        /// <summary>
        /// 要发送的图片的说明
        /// </summary>
        private readonly TelegramMessageBuilder? Caption;

        /// <summary>
        /// 动作按钮
        /// </summary>
        private readonly ActionButtonResult[]? ButtonResults;

        /// <summary>
        /// 发送单张图片
        /// </summary>
        /// <param name="photoPath"></param>
        /// <param name="caption"></param>
        /// <param name="buttonResults"></param>
        public PhotoMessageResult(string photoPath, TelegramMessageBuilder? caption = null, ActionButtonResult[]? buttonResults = null)
        {
            PhotoPaths = new string[] { photoPath };
            Caption = caption;
            ButtonResults = buttonResults;
        }

        /// <summary>
        /// 发送图片组
        /// </summary>
        /// <remarks>
        /// 发送图片组，只有第一张图片有对应说明，不支持动作按钮
        /// </remarks>
        /// <param name="photoPaths">图片组路径</param>
        /// <param name="caption">说明信息</param>
        public PhotoMessageResult(string[] photoPaths, TelegramMessageBuilder? caption = null)
        {
            PhotoPaths = photoPaths;
            Caption = caption;
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
            return new SendPhotoRequest(chatID!, InputFile.FromStream(Files[0], Path.GetFileName(PhotoPaths[0])))
            {
                Caption = Caption?.ToString(),
                ParseMode = Caption?.ParseMode,
                ReplyMarkup = ButtonResults == null ? null : new InlineKeyboardMarkup(GetInlineKeyboardButtons(context, ButtonResults)),
            };
        }
    }
}
