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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller.Results
{
    /// <summary>
    /// 发送带有图片的消息
    /// </summary>
    public class PhotoMessageResult : ActionResult
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
        /// <param name="BotClient"></param>
        /// <param name="ServiceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Message[]> ExecuteResultAsync(TelegramActionContext context, ITelegramBotClient BotClient, IServiceProvider ServiceProvider, CancellationToken cancellationToken)
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
                    {
                        var resultMessage = await context.TelegramBotClient.SendPhotoAsync(context.ChatId!, InputFile.FromStream(bufferedStream), caption: Caption?.ToString(),
                            parseMode: Caption?.ParseMode, cancellationToken: cancellationToken);
                        
                        return new Message[] { resultMessage };
                    }
                    else
                    {
                        var resultMessage = await context.TelegramBotClient.SendPhotoAsync(context.ChatId!, InputFile.FromStream(bufferedStream), caption: Caption?.ToString(),
                            parseMode: Caption?.ParseMode, replyMarkup: new InlineKeyboardMarkup(buttons), cancellationToken: cancellationToken);

                        return new Message[] { resultMessage };
                    }
                }
            }
            else
            {
                await using (var streams = new ListAsyncDisposable<BufferedStream>())
                {
                    foreach (var stream in PhotoPaths)
                        streams.Add(new BufferedStream(new FileStream(stream, FileMode.Open), Consts.BUFFED_STREAM_CACHE_128KB));

                    return await context.TelegramBotClient.SendMediaGroupAsync(context.ChatId!,
                        streams.Select(x => new InputMediaPhoto(InputFile.FromStream(x)) { Caption = Caption?.ToString(), ParseMode = Caption?.ParseMode }).ToArray(),
                        cancellationToken: cancellationToken);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task ExecuteChatActionAsync(TelegramActionContext context, CancellationToken cancellationToken) =>
            context.TelegramBotClient.SendChatActionAsync(context.ChatId!, ChatAction.UploadPhoto, cancellationToken: cancellationToken);
    }
}
