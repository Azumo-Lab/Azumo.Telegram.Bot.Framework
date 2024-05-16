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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 
    /// </summary>
    internal class PhotoMessageResult : MessageResult
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] PhotoPaths;

        /// <summary>
        /// 
        /// </summary>
        private readonly string? Caption;

        /// <summary>
        /// 
        /// </summary>
        private readonly ActionButtonResult[]? ButtonResults;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoPath"></param>
        /// <param name="caption"></param>
        /// <param name="buttonResults"></param>
        public PhotoMessageResult(string photoPath, string? caption = null, ActionButtonResult[]? buttonResults = null)
        {
            PhotoPaths = new string[] { photoPath };
            Caption = caption;
            ButtonResults = buttonResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoPaths"></param>
        /// <param name="caption"></param>
        /// <param name="buttonResults"></param>
        public PhotoMessageResult(string[] photoPaths, string? caption = null, ActionButtonResult[]? buttonResults = null)
        {
            PhotoPaths = photoPaths;
            Caption = caption;
            ButtonResults = buttonResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task ExecuteResultAsync(TelegramActionContext context, CancellationToken cancellationToken)
        {
            var buttons = new List<InlineKeyboardButton>();
            if (ButtonResults != null && ButtonResults.Length > 0)
            {
                var callbackManager = context.ServiceProvider.GetRequiredService<ICallBackManager>();
                foreach (var item in ButtonResults)
                    buttons.Add(callbackManager.CreateCallBackButton(item));
            }
            if (PhotoPaths.Length == 1)
            {
                await using (var bufferedStream = new BufferedStream(new FileStream(PhotoPaths[0], FileMode.Open), Consts.BUFFED_STREAM_CACHE_128KB))
                {
                    if (buttons.Count == 0)
                        await context.TelegramBotClient.SendPhotoAsync(context.ChatId, InputFile.FromStream(bufferedStream), caption: Caption, 
                            parseMode: Types.Enums.ParseMode.Html, cancellationToken: cancellationToken);
                    else
                        await context.TelegramBotClient.SendPhotoAsync(context.ChatId, InputFile.FromStream(bufferedStream), caption: Caption, 
                            replyMarkup: new InlineKeyboardMarkup(buttons), cancellationToken: cancellationToken);
                }
            }
            else
            {
                await using (var streams = new ListAsyncDisposable<BufferedStream>())
                {
                    foreach (var stream in PhotoPaths)
                        streams.Add(new BufferedStream(new FileStream(stream, FileMode.Open), Consts.BUFFED_STREAM_CACHE_128KB));

                    await context.TelegramBotClient.SendMediaGroupAsync(context.ChatId,
                        streams.Select(x => new InputMediaPhoto(InputFile.FromStream(x)) { Caption = Caption, ParseMode = Types.Enums.ParseMode.Html }).ToArray(),
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
