using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telegram.Bot.Framework.TelegramMessage;

namespace Telegram.Bot.Framework
{
    public abstract class TelegramController
    {
        /// <summary>
        /// 执行调用
        /// </summary>
        public void Invoke()
        {

        }

        /// <summary>
        /// 发送一条消息
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected virtual ICommandResult SendTextMessage(string Message)
        {
            return default;
        }

        /// <summary>
        /// 发送一张图片
        /// </summary>
        /// <returns></returns>
        protected virtual ICommandResult SendPhoto(string PhotoPath)
        {
            return default;
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
