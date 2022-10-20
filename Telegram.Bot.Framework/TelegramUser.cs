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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Telegram的用户
    /// </summary>
    public sealed class TelegramUser
    {
        internal TelegramUser(User user, long chatID)
        {
            Id = user.Id;
            IsBot = user.IsBot;
            FirstName = user.FirstName;
            LanguageCode = user.LanguageCode;
            LastName = user.LastName;
            Username = user.Username;
            CanJoinGroups = user.CanJoinGroups;
            CanReadAllGroupMessages = user.CanReadAllGroupMessages;
            SupportsInlineQueries = user.SupportsInlineQueries;
            ChatID = chatID;
        }

        internal TelegramUser()
        {

        }

        public Dictionary<string, string> UserData { get; } = new Dictionary<string, string>();

        /// <summary>
        /// 用户ID
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// 用户是否是Bot
        /// </summary>
        public bool IsBot { get; internal set; }

        /// <summary>
        /// 用户第一个名
        /// </summary>
        public string FirstName { get; internal set; }

        /// <summary>
        /// 用户最后一个名
        /// </summary>
        public string LastName { get; internal set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; internal set; }

        /// <summary>
        /// 语言Code
        /// </summary>
        public string LanguageCode { get; internal set; }

        /// <summary>
        /// 是否能够加入群组
        /// </summary>
        public bool? CanJoinGroups { get; internal set; }

        public bool? CanReadAllGroupMessages { get; internal set; }

        public bool? SupportsInlineQueries { get; internal set; }

        /// <summary>
        /// 用户ChatID
        /// </summary>
        public long ChatID { get; set; }

        /// <summary>
        /// 用户是否被Bot屏蔽
        /// </summary>
        public bool IsBlock { get; set; }

        /// <summary>
        /// 用户的权限
        /// </summary>
        public AuthenticationRole AuthenticationRole { get; set; }
    }
}
