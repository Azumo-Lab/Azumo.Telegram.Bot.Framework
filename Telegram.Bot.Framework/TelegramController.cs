using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.TelegramMessage;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework
{
    public abstract class TelegramController
    {
        protected TelegramContext TelegramContext { get; private set; }
        /// <summary>
        /// 执行调用
        /// </summary>
        public void Invoke(TelegramContext context)
        {
            TelegramContext = context;
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
        protected virtual ICommandResult SendPhoto(PhotoInfo Photo)
        {
            return default;
        }

        /// <summary>
        /// 发送多张图片
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendPhotos(IEnumerable<PhotoInfo> Photos)
        {
            return default;
        }

        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual ICommandResult SendFile(byte[] File, string FileName)
        {
            return default;
        }

        /// <summary>
        /// 发送一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected virtual ICommandResult SendFile(string FilePath)
        {
            if (File.Exists(FilePath))
                return SendFile(File.ReadAllBytes(FilePath), Path.GetFileName(FilePath));
            throw new FileNotFoundException($"未找到文件 {FilePath}");
        }

        /// <summary>
        /// 发送一个表情贴纸
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendSticker()
        {
            return default;
        }

        /// <summary>
        /// 发送一个音频
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendAudio()
        {
            return default;
        }

        /// <summary>
        /// 发送一段视频
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendVideo()
        {
            return default;
        }

        /// <summary>
        /// 发送一个联系人
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendContact()
        {
            return default;
        }

        /// <summary>
        /// 发送一个地图坐标
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendVenue()
        {
            return default;
        }
    }
}
