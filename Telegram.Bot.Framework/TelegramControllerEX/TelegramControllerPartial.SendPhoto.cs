//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.TelegramMessage;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.TelegramControllerEX
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TelegramControllerPartial
    {
        /// <summary>
        /// 发送一张图片
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendPhoto(string PhotoPath)
        {
            await TelegramContext.BotClient.SendPhotoAsync(
                chatId: TelegramContext.ChatID,
                photo: new Types.InputFiles.InputOnlineFile(File.OpenRead(PhotoPath), Path.GetFileName(PhotoPath))
                );
        }

        /// <summary>
        /// 发送一张图片
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendPhoto(string PhotoPath, string Message)
        {
            await TelegramContext.BotClient.SendPhotoAsync(
                chatId: TelegramContext.ChatID,
                photo: new Types.InputFiles.InputOnlineFile(File.OpenRead(PhotoPath), Path.GetFileName(PhotoPath)),
                caption: Message
                );
        }

        /// <summary>
        /// 发送一张图片
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendPhoto(string PhotoPath, string Message, IEnumerable<InlineKeyboardButton> keyboardButton)
        {
            await TelegramContext.BotClient.SendPhotoAsync(
                chatId: TelegramContext.ChatID,
                photo: new Types.InputFiles.InputOnlineFile(File.OpenRead(PhotoPath), Path.GetFileName(PhotoPath)),
                caption: Message,
                replyMarkup: new InlineKeyboardMarkup(keyboardButton)
                );
        }

        /// <summary>
        /// 发送一张图片
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendPhoto(PhotoInfo Photo)
        {

        }

        /// <summary>
        /// 发送多张图片
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendPhotos(IEnumerable<PhotoInfo> Photos)
        {

        }
    }
}
