//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
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
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.ControllerManger;
using Telegram.Bot.Framework.TelegramMessage;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Framework
{
    public abstract class TelegramController
    {
        protected TelegramContext TelegramContext { get; private set; }

        internal IServiceProvider ServiceProvider;
        /// <summary>
        /// 执行调用
        /// </summary>
        internal async Task Invoke(TelegramContext context, IServiceProvider serviceProvider,string CommandName)
        {
            TelegramContext = context;
            ServiceProvider = serviceProvider;

            IDelegateManger delegateManger = ServiceProvider.GetService<IDelegateManger>();
            IParamManger paramManger = serviceProvider.GetService<IParamManger>();

            Delegate action = delegateManger.CreateDelegate(CommandName, this);
            object[] Params = paramManger.GetParam(context);
            if (Params == null || Params.Length == 0)
                await Task.Run(() => action.DynamicInvoke());
            else
                await Task.Run(() => action.DynamicInvoke(Params));
        }

        /// <summary>
        /// 发送一条消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected virtual async Task SendTextMessage(string Message)
        {
            await TelegramContext.BotClient.SendTextMessageAsync(
                chatId: TelegramContext.ChatID,
                Message, ParseMode.MarkdownV2
                );
        }

        /// <summary>
        /// 发送一条消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected virtual async Task SendTextMessage(string Message, IEnumerable<InlineKeyboardButton> keyboardButton)
        {
            await TelegramContext.BotClient.SendTextMessageAsync(
                chatId: TelegramContext.ChatID,
                Message, ParseMode.MarkdownV2
                , replyMarkup: new InlineKeyboardMarkup(keyboardButton)
                );
        }

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

        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual async Task SendFile(byte[] File, string FileName)
        {
            
        }

        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual async Task SendFile(string FilePath)
        {
            if (File.Exists(FilePath))
                await SendFile(File.ReadAllBytes(FilePath), Path.GetFileName(FilePath));
            else
                throw new FileNotFoundException($"未找到文件 {FilePath}");
        }

        /// <summary>
        /// 发送一个表情贴纸
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendSticker()
        {
            
        }

        /// <summary>
        /// 发送一个音频
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendAudio()
        {
            
        }

        /// <summary>
        /// 发送一段视频
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendVideo()
        {
            
        }

        /// <summary>
        /// 发送一个联系人
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendContact()
        {
            
        }

        /// <summary>
        /// 发送一个地图坐标
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SendVenue()
        {
            
        }
    }
}
