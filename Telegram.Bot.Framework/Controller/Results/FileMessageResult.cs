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
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class FileMessageResult : ActionResult
    {
        private readonly string? File_ID;
        
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
                Files.Add(new BufferedStream(new FileStream(file, FileMode.Open), Consts.BUFFED_STREAM_CACHE_256KB));
            else 
                File_ID = file;

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
        /// <param name="BotClient"></param>
        /// <param name="ServiceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Message[]> ExecuteResultAsync(TelegramActionContext context, ITelegramBotClient BotClient, IServiceProvider ServiceProvider, CancellationToken cancellationToken)
        {
            var resultMessage = string.IsNullOrEmpty(File_ID) && !Files.IsEmpty()
                ? await BotClient.SendDocumentAsync(context.ChatId!,
                    InputFile.FromStream(Files[0]), caption: Text?.ToString(), parseMode: Text?.ParseMode, cancellationToken: cancellationToken)
                : await BotClient.SendDocumentAsync(context.ChatId!,
                    InputFile.FromFileId(File_ID!),
                    caption: Text?.ToString(), parseMode: Text?.ParseMode,
                    cancellationToken: cancellationToken);

            return new Message[] { resultMessage };
        }
    }
}
