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

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class FileMessageResult : ActionResult<Message>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string? FileName_FileID;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FileMessageResult(string file, TelegramMessageBuilder? message = null)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException(file);

            if(System.IO.File.Exists(file))
            {
                Files.Add(file.OpenBufferedStream());
                FileName_FileID = Path.GetFileName(file);
            }
            else 
                FileName_FileID = file;

            if (message != null)
                Text = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken) => 
            await context.TelegramBotClient.SendChatActionAsync(context.ChatId!, ChatAction.UploadDocument, cancellationToken: cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override IRequest<Message> ExecuteResultAsync(TelegramActionContext context)
        {
            var chatID = context.ChatId;
            return Files.IsEmpty()
                ? new SendDocumentRequest(chatID!, InputFile.FromFileId(FileName_FileID!))
                {
                    Caption = Text?.ToString(),
                    ParseMode = Text?.ParseMode
                }
                : new SendDocumentRequest(chatID!, InputFile.FromStream(Files[0], FileName_FileID))
                {
                    Caption = Text?.ToString(),
                    ParseMode = Text?.ParseMode
                };
        }
    }
}
