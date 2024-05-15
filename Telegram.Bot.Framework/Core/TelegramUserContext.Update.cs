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

using System;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core
{
    public sealed partial class TelegramContext : Update
    {
        /// <summary>
        /// <see cref="Telegram.Bot.Types.Update"/> 对象更新时触发的事件
        /// </summary>
        public event EventHandler<Update>? Update;

        /// <summary>
        /// 将 <see cref="Telegram.Bot.Types.Update"/> 对象的属性复制到当前对象
        /// </summary>
        /// <param name="update">要进行复制的对象</param>
        public void CopyFrom(Update update)
        {
            Message = update.Message;
            InlineQuery = update.InlineQuery;
            MyChatMember = update.MyChatMember;
            CallbackQuery = update.CallbackQuery;
            ChannelPost = update.ChannelPost;
            ChatJoinRequest = update.ChatJoinRequest;
            ChatMember = update.ChatMember;
            ChosenInlineResult = update.ChosenInlineResult;
            EditedChannelPost = update.EditedChannelPost;
            EditedMessage = update.EditedMessage;
            Poll = update.Poll;

            Update?.Invoke(this, update);
        }
    }
}
