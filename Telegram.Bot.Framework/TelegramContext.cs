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
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 机器人的各类信息
    /// </summary>
    public class TelegramContext
    {
        /// <summary>
        /// 机器人接口
        /// </summary>
        public ITelegramBotClient BotClient { get; internal set; }

        /// <summary>
        /// 接收的信息
        /// </summary>
        public Update Update { get; internal set; }

        /// <summary>
        /// Token
        /// </summary>
        public CancellationToken CancellationToken { get; internal set; }

        /// <summary>
        /// 获取ChatID
        /// </summary>
        public long ChatID => GetChatID();

        internal TelegramContext() { }

        /// <summary>
        /// 获取ChatID(相当于用户ID)
        /// </summary>
        /// <returns></returns>
        private long GetChatID()
        {
            return Update.Type switch
            {
                Types.Enums.UpdateType.CallbackQuery => Update.CallbackQuery.Message.Chat.Id,
                _ => Update.Message.Chat.Id,
            };
        }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <returns></returns>
        internal string GetCommand()
        {
            string command;
            if (!string.IsNullOrEmpty(command = Update.Message?.Text))
                if (!command.StartsWith('/'))
                    command = null;
            return command.ToLower();
        }
    }
}
