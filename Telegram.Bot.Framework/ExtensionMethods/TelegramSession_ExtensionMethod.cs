//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public static class TelegramSession_ExtensionMethod
    {
        #region 获取Session中的值
 
        public static long? GetChatID(this ITelegramSession session)
        {
            return session.User?.ChatID;
        }

        public static bool GetChatID(this ITelegramSession session, out long chatID)
        {
            long? vlChatID = GetChatID(session);
            if (vlChatID.HasValue)
                chatID = vlChatID.Value;
            else
                chatID = long.MinValue;
            return vlChatID.HasValue;
        }

        public static string GetCommand(this ITelegramSession session)
        {
            if (session.Update.Type != Types.Enums.UpdateType.Message)
                return default!;

            if (session.Update.Message?.Type != Types.Enums.MessageType.Text)
                return default!;

            string text = session.Update.Message.Text ?? string.Empty;
            List<MessageEntity> messageEntities = session.Update.Message.Entities?.ToList() ?? new List<MessageEntity>();
            List<string> messageEntitiesVal = session.Update.Message.EntityValues?.ToList() ?? new List<string>();
            MessageEntity messageEntity;
            if ((messageEntity = messageEntities.FirstOrDefault()) == null)
                return default!;

            if (messageEntity.Type == Types.Enums.MessageEntityType.BotCommand)
                return messageEntitiesVal.FirstOrDefault();

            return default!;
        }

        #endregion

        #region 文本消息的发送
        public static async Task SendTextMessageAsync(this ITelegramSession session, string message)
        {
            if(!session.GetChatID(out long chatID))
                return;

            await session.BotClient.SendTextMessageAsync(chatID, message, Types.Enums.ParseMode.MarkdownV2);
        }
        #endregion

        #region 图片的发送
        public static async Task SendPhoto(this ITelegramSession session, string PhotoPath)
        {
            await SendPhoto(session, PhotoPath, null!);
        }

        public static async Task SendPhoto(this ITelegramSession session, string PhotoPath, string message)
        {
            if (!session.GetChatID(out long chatID))
                return;

            if (!System.IO.File.Exists(PhotoPath))
                return;

            await session.BotClient.SendPhotoAsync(chatID,
                new Types.InputFiles.InputOnlineFile(new FileStream(PhotoPath, FileMode.OpenOrCreate), Path.GetFileName(PhotoPath)), 
                message, 
                Types.Enums.ParseMode.MarkdownV2);
        }

        public static async Task SendPhoto(this ITelegramSession session, PhotoSize photoSize)
        {
            await SendPhoto(session, photoSize, null!);
        }

        public static async Task SendPhoto(this ITelegramSession session, PhotoSize photoSize, string message)
        {
            if (!session.GetChatID(out long chatID))
                return;

            await session.BotClient.SendPhotoAsync(chatID, new Types.InputFiles.InputOnlineFile(photoSize.FileId), message, Types.Enums.ParseMode.MarkdownV2);
        }
        #endregion
    }
}
