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

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Models
{
    /// <summary>
    /// 消息实体信息
    /// </summary>
    public sealed class MessageEntityInfo
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public MessageEntityInfo(MessageEntity messageEntity, string value)
        {
            Type = messageEntity.Type;
            Offset = messageEntity.Offset;
            Length = messageEntity.Length;
            Url = messageEntity.Url;
            User = messageEntity.User;
            Language = messageEntity.Language;
            CustomEmojiId = messageEntity.CustomEmojiId;

            Value = value ?? string.Empty;
        }

        /// <summary>
        /// 消息值
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageEntityType Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        public string? Url { get; }

        /// <summary>
        /// 
        /// </summary>
        public User? User { get; }

        /// <summary>
        /// 
        /// </summary>
        public string? Language { get; }

        /// <summary>
        /// 
        /// </summary>
        public string? CustomEmojiId { get; }
    }
}
